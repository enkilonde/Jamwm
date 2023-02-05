using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Objects/SoundData", order = 2)]
public class SoundData : ScriptableObject
{
    public SoundInfo MusicHome => _musicHome;
    public SoundInfo MusicBattle => _musicBattle;
    public List<SoundInfo> Sounds => _sounds;
    
    [SerializeField]private SoundInfo _musicHome;
    [SerializeField]private SoundInfo _musicBattle;
    
    [SerializeField]private List<SoundInfo> _sounds;
    
#if UNITY_EDITOR

    [Button]
    private void GenerateMissingSounds()
    {
        foreach (SoundInfo.SoundType soundType in Enum.GetValues(typeof( SoundInfo.SoundType)))
        {
            if (_sounds.FirstOrDefault(x=>x.Type== soundType)==null)
            {
                SoundInfo soundInfo = new SoundInfo();
                soundInfo.Type = soundType;
                soundInfo.SetVolumeToOne();
                soundInfo.SetPitchBNoVariation();
                
                _sounds.Add(soundInfo);
            }
        }
    }
#endif
}
