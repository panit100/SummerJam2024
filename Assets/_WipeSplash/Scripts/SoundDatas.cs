using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundData", menuName = "Sound/SoundData", order = 0)]
public class SoundDatas : ScriptableObject
{
    public List<AudioClip> allBGM;
    public List<AudioClip> allSFX;
    public List<AudioMixerGroup> audioMixers;
}

