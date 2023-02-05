using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public bool IsSoundOn => _isSoundOn;
    
    [SerializeField] private SoundData _soundData;
    
    [SerializeField, Range(0f,1f)] private float _fadedVolume;
    [SerializeField, Range(0f,1f)] private float _fadeDuration;
    [SerializeField] private bool _debug;
    [SerializeField] private AudioSource _sourceObject;
    [SerializeField] private SoundInfo.SoundType _defaultSound = SoundInfo.SoundType.Missing;

    private AudioSource _musicHome, _musicBattle;
    private AudioSource _currentMusic;

    public enum MusicType
    {
        Uninitialized =-1,
        Home,
        Battle,
    }
    public enum MusicState
    {
        Uninitialized =-1,
        Stopped,
        Playing,
        Dampened
    }
    
    private MusicType _musicType = MusicType.Uninitialized;
    private MusicState _musicState = MusicState.Uninitialized;
    
    private Dictionary<SoundInfo.SoundType, SoundInfo> _soundDictionary;
    private Dictionary<MusicType, SoundInfo> _musicDictionary;
    
    private bool _isSoundOn;
    private const string SoundDataFileName = "sound_data";
    private const string SoundOnKey = "sound_on";
    
    public void Init()
    {
        DebugPrint("[SFX] Init");

        CreateDictionaries();

        _isSoundOn = PlayerPrefs.GetInt("sound_on", 1)==1;
        
        _musicHome = GetMusicSource(MusicType.Home);
        _musicBattle = GetMusicSource(MusicType.Battle);
    }

    public void EnableSound()
    {
        _isSoundOn = true;
        PlayerPrefs.SetInt("sound_on", 1);
        SetMusicState(_musicState);
    }

    public void DisableSound()
    {
        _isSoundOn = false;
        PlayerPrefs.SetInt("sound_on", 0);
        StartCoroutine(MusicFadeTo(0, false,0));
    }

    [Button]
    public void SetMusicType(MusicType music)
    {
        if (_musicType==music) return;

        DebugPrint("[SFX] Set music type from: "+_musicType+" to : "+music );
        
        if (_musicType!= MusicType.Uninitialized)
            SetMusicState(MusicState.Stopped);
        
        _musicType = music;

        switch (_musicType)
        {
            case MusicType.Home:
                _currentMusic = GetMusicSource(MusicType.Home);
                break;
            case MusicType.Battle:
                _currentMusic = GetMusicSource(MusicType.Battle);
                break;
        }
        
        SetMusicState(MusicState.Playing);
    }
    
    [Button]
    public void SetMusicState(MusicState state)
    {
        DebugPrint("[SFX] Set music state from: "+_musicState+" to "+state);
        _musicState = state;
        
        if(!_isSoundOn) return;
        switch (state)
        {
            case MusicState.Stopped:
                StartCoroutine(MusicFadeTo(0f, false,_fadeDuration));
                break;
            case MusicState.Playing:
                StartCoroutine(MusicFadeTo(_musicDictionary[_musicType].Volume, true,_fadeDuration));
                break;
            case MusicState.Dampened:
                StartCoroutine(MusicFadeTo( _fadedVolume, true,_fadeDuration));
                break;
        }
    }
    
    private IEnumerator MusicFadeTo(float volume, bool play, float duration = 0.5f)
    {
        var source = _currentMusic;
        DebugPrint("[SFX] Sound " + source.clip.name + "- Volume : " + volume + "- Play : " + play);
        if (play && !source.isPlaying) source.Play();

        float baseVolume = source.volume;

        for (float time = 0f; time < duration; time = Mathf.MoveTowards(time, duration, Time.deltaTime))
        {
            source.volume = Mathf.Lerp(baseVolume, volume, time / duration);
            yield return null;
        }

        if (!play) source.Stop();
    }
    
    [Button] public void PlaySound(SoundInfo.SoundType type)
    {
        if(!_isSoundOn) return;
        SoundInfo soundToPlay =null;
    
        if (_soundDictionary.ContainsKey(type))
        {
            var sound = _soundDictionary[type];
            soundToPlay = (sound.Clip != null) ? sound : _soundDictionary[_defaultSound];
        }
        else
        {
            soundToPlay = _soundDictionary[_defaultSound];
        }
        
        var source = PrepareSound(soundToPlay);
        source.Play();
        Destroy(source, source.clip.length);
    }
    
    private AudioSource PrepareSound(SoundInfo sound, int forceClip = -1)
    {
        DebugPrint("[SFX] Prepare "+sound.Clip.name);

        var source = GetAudioSource();
        if (forceClip==-1)
            source.clip = sound.Clip;
        else
            source.clip = sound.ClipByIndex(forceClip);

        source.pitch = sound.Pitch;
        source.loop = sound.Loop;
        source.volume = sound.Volume;
        return source;
    }
    
    private AudioSource GetAudioSource()
    {
        return Instantiate(_sourceObject).GetComponent<AudioSource>();
    }
    private void CreateDictionaries()
    {
        _soundDictionary = new Dictionary<SoundInfo.SoundType, SoundInfo>();
        foreach (var soundInfo in _soundData.Sounds)
        {
            _soundDictionary.Add(soundInfo.Type, soundInfo);
        }

        _musicDictionary = new Dictionary<MusicType, SoundInfo>();
        _musicDictionary.Add(MusicType.Home, _soundData.MusicHome);
        _musicDictionary.Add(MusicType.Battle, _soundData.MusicBattle);
    }

    private AudioSource GetMusicSource(MusicType type)
    {
        switch (type)
        {
            case MusicType.Uninitialized:
                break;
            case MusicType.Home:
                if (_musicHome!=null)
                    DestroyImmediate(_musicHome);
                return PrepareSound(_soundData.MusicHome);
            case MusicType.Battle:
                if (_musicBattle!=null)
                    DestroyImmediate(_musicBattle);
                return PrepareSound(_soundData.MusicBattle);
        }
        return null;
    }

    private void DebugPrint(string text)
    {
        if (_debug)
        {
            Debug.Log(text);
        }
    }
}
