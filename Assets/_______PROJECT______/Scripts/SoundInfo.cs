using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SoundInfo
{
    public enum SoundType
    {
        Missing = -1,
        Nothing,
        ButtonUIScroll,
        ButtonUITapIn,
        ButtonUITapOut,
        CloseAttack = 10,
        RangeAttack = 15,
        AttackCharge,
        AttackChargeEnemy,
        HitPlayer = 20,
        HitEnemy = 25,
        Loot,
        Footsteps,
        Dash,
        DeathPlayer,
        DeathEnemy,
        DoorOpen,
    }
    
    public AudioClip Clip
    {
        get
        {
            if (Clips==null|| Clips.Count==0 || Clips[0]==null)
                return null;
            return Clips[Random.Range(0, Clips.Count)];
        }
    }

    public float Pitch => Random.Range(_pitchRange.x, _pitchRange.y);
    public float Volume => _volume;
    
    [SerializeField, Title("@GetSoundTitle()"),GUIColor("GetSoundColor")]private List<AudioClip> Clips;
    [SerializeField,FoldoutGroup("Properties"), HideIf("Loop")]public SoundType Type;
    [SerializeField,FoldoutGroup("Properties")]public bool Loop = false;
    [SerializeField, BoxGroup("Properties/Volume"), HorizontalGroup("Properties/Volume/Set"), Range(0f,1f)]private float _volume;
    [SerializeField, BoxGroup("Properties/Pitch"), MinMaxSlider(0f,3f, true)]private Vector2 _pitchRange;

    public AudioClip ClipByIndex(int index)
    {
        if (Clips==null|| Clips.Count==0 || Clips[0]==null)
            return null;
        return Clips[index % Clips.Count]; 

    }
#if UNITY_EDITOR
    private string GetSoundTitle()
    {
        string returnString = Type.ToString()+" : ";
        if (Clips==null|| Clips.Count==0)
        {
            returnString+="No clip linked";
        }
        else
        {
            foreach (var clip in Clips)
            {
                if (clip!=null)
                {
                    returnString+=clip.name+" - ";

                }
                else
                {
                    returnString+="No clip linked";
                }
            }

        }
        return returnString;
    }
    
    private Color GetSoundColor()
    {
        if (Clips==null|| Clips.Count==0) return Color.red;
        return Color.white;
    }

    [Button, HorizontalGroup("Properties/Volume/Set"), LabelText("Set to 1")] public void SetVolumeToOne() { _volume = 1f; }
    [Button, HorizontalGroup("Properties/Pitch/Variation"), LabelText("No variation")] public void SetPitchBNoVariation() { _pitchRange = Vector2.one; }
    [Button, HorizontalGroup("Properties/Pitch/Variation"), LabelText("Small variation")] public void SetPitchSmallVariation() { _pitchRange = new Vector2(0.9f, 1.1f); }
    [Button, HorizontalGroup("Properties/Pitch/Variation"), LabelText("Big variation")] public void SetPitchBigVariation() { _pitchRange = new Vector2(0.5f, 2f); }
#endif
}