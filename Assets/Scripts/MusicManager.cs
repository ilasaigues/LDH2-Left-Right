using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public AudioMixer mixer;
    public LevelManager levelManager;

    public float musicSyncRefreshRate = 2;
    public List<AudioSource> musicSources = new List<AudioSource>();

    const string KEY_LOOP1 = "VolLoop1";
    const string KEY_LOOP2 = "VolLoop2";
    const string KEY_LOOP3 = "VolLoop3";
    const string KEY_LOOP4 = "VolLoop4";
    // Start is called before the first frame update
    void Start()
    {

    }

    float timeSinceLastSync;

    // Update is called once per frame
    void Update()
    {
        int currentLevel = levelManager.CurrentLevel;

        float loopVol;
        mixer.GetFloat(KEY_LOOP1, out loopVol);
        mixer.SetFloat(KEY_LOOP1, Mathf.Lerp(loopVol, currentLevel <= 0 ? 0 : -80, .3333f));

        mixer.GetFloat(KEY_LOOP2, out loopVol);
        mixer.SetFloat(KEY_LOOP2, Mathf.Lerp(loopVol, currentLevel < 0 ? 0 : -80, .3333f));

        mixer.GetFloat(KEY_LOOP3, out loopVol);
        mixer.SetFloat(KEY_LOOP3, Mathf.Lerp(loopVol, currentLevel < -1 ? 0 : -80, .3333f));

        mixer.GetFloat(KEY_LOOP4, out loopVol);
        mixer.SetFloat(KEY_LOOP4, Mathf.Lerp(loopVol, currentLevel < -3 ? 0 : -80, .3333f));


        if (timeSinceLastSync > musicSyncRefreshRate)
        {
            int samples = musicSources[0].timeSamples;
            for (int i = 1; i < musicSources.Count; i++)
            {
                musicSources[0].timeSamples = samples % musicSources[0].clip.samples;
            }
            timeSinceLastSync = 0;
        }
        timeSinceLastSync += Time.deltaTime;
    }
}
