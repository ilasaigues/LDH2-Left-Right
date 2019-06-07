using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PauseMenu : MonoBehaviour
{

    public Button resumeButton;
    public Image volumeSlider;
    public Image mask;
    public CharacterController character;
    public BlurEffect cameraBlurEffect;

    float _targetMaskFill = 0;

    bool _open = false;

    private SoundManager _soundManager;
    public void Resume()
    {
        _open = false;
        _targetMaskFill = 0;
        mask.fillClockwise = false;
        character.LockControls(false);

        foreach (Button button in GetComponentsInChildren<Button>())
        {
            button.interactable = false;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Open()
    {
        if (_soundManager == null) _soundManager = Director.GetManager<SoundManager>();
        SetVolume(Mathf.Round(_soundManager.volume * 10) / 10);

        character.LockControls();

        mask.fillClockwise = true;
        _targetMaskFill = 1;
        foreach (Button button in GetComponentsInChildren<Button>())
        {
            button.interactable = true;
        }
    }

    public void MoreVolume()
    {
        SetVolume(Mathf.Clamp01(_soundManager.volume + .1f));
    }
    public void LessVolume()
    {
        SetVolume(Mathf.Clamp01(_soundManager.volume - .1f));
    }

    private void OnEnable()
    {
        resumeButton.Select();
        resumeButton.OnSelect(null);
    }

    void SetVolume(float vol)
    {
        Director.GetManager<SoundManager>().SetMasterVolumeScalar(vol);
        volumeSlider.fillAmount = vol;
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _open = !_open;
        }

        if (_open)
        {
            Open();
        }
        else
        {
            Resume();
        }

        mask.fillAmount = Mathf.Lerp(mask.fillAmount, _targetMaskFill, .2f);
        cameraBlurEffect.intensity = Mathf.Lerp(cameraBlurEffect.intensity, _targetMaskFill, .2f);
    }

}
