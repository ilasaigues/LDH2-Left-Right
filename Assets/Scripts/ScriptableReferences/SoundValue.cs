using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
[Serializable]
[CreateAssetMenu(fileName = "SoundValue", menuName = "Scriptables/Values/Sound")]
public class SoundValue : ScriptableValue<AudioClip>
{
    [Range(0, 1)]
    public float volume = 1;
    public bool loop = false;
    public AudioMixerGroup mixerGroup;
    [Range(-3, 3)]
    public float pitch = 1;
}
[Serializable]
public class SoundReference : ScriptableReference<AudioClip, SoundValue>
{
}