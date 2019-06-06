using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Manager
{
    private List<AudioSource> _usedSources = new List<AudioSource>();
    private List<AudioSource> _freeSources = new List<AudioSource>();

    public void PlaySound(SoundValue sound)
    {
        AudioSource source;
        if (_freeSources.Count == 0)
        {
            source = new GameObject().AddComponent<AudioSource>();
            source.transform.parent = transform;
        }
        else
        {
            source = _freeSources[0];
            _freeSources.RemoveAt(0);
        }
        _usedSources.Add(source);
        source.gameObject.name = "Sound: " + sound.name;
        AssignSoundToSource(source, sound);
        source.Play();
    }

    void AssignSoundToSource(AudioSource source, SoundValue sound)
    {
        source.clip = sound.value;
        source.volume = sound.volume;
        source.loop = sound.loop;
        source.pitch = sound.pitch;
        source.outputAudioMixerGroup = sound.mixerGroup;
    }
    private void Update()
    {
        for (int i = _usedSources.Count - 1; i >= 0; i--)
        {
            if (!_usedSources[i].isPlaying)
            {
                _usedSources[i].gameObject.name = "[Free AudioSource]";
                _freeSources.Add(_usedSources[i]);
                _usedSources.RemoveAt(i);

            }
        }
    }

}