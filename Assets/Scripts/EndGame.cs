using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour, ICollidable
{
    public ParticleSystem explosion;
    public float implosionBeginTime;
    public float implosionCollapseTime;
    public float beginTextFadeTime;
    public float endTextFadeTime;
    public float closeTime;
    float _endTimer = -1;
    CharacterController character;
    public UnityEngine.UI.Text creditsText;

    public SoundValue partyHornSound;
    public SoundValue ominousSound;

    private AudioSource _ominousSoundSource;
    private float _runTimer = -1;
    void Start()
    {
        character = FindObjectOfType<CharacterController>();
    }

    public void CollidedWithCharacterController(CharacterController characterController)
    {
        if (_endTimer >= 0) return;
        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponent<ParticleSystem>());
        Instantiate(explosion, transform.position, Quaternion.identity);
        Director.GetManager<SoundManager>().PlaySound(partyHornSound);
        _ominousSoundSource = Director.GetManager<SoundManager>().PlaySound(ominousSound);
        _endTimer = 0;

        float minutes = Mathf.Floor(_runTimer / 60);
        float seconds = Mathf.Floor(_runTimer % 60);
        float milliseconds = Mathf.Floor((_runTimer % 1) * 100);
        creditsText.text = string.Format(creditsText.text, minutes + ":" + seconds + "." + milliseconds);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterController charCon = collision.GetComponent<CharacterController>();
        if (charCon != null)
        {
            CollidedWithCharacterController(charCon);
        }
    }

    void Update()
    {
        if (_endTimer >= 0) _endTimer += Time.deltaTime;
        if (_runTimer >= 0)
        {
            _runTimer += Time.deltaTime;
            Debug.Log(_runTimer);
        }
    }

    void FixedUpdate()
    {


        if (_endTimer > implosionBeginTime)
        {
            float lerpFactor = Mathf.Clamp01((_endTimer - implosionBeginTime) / (implosionCollapseTime - implosionBeginTime));
            character.Lock();
            character.transform.position = Vector3.Lerp(character.transform.position, transform.position, lerpFactor * lerpFactor);

            Camera.main.GetComponent<SmoothCameraFollow>().verticalLock = true;
            Camera.main.GetComponent<SmoothCameraFollow>().lerp = .1f;
            _ominousSoundSource.volume = lerpFactor;
            Camera.main.GetComponent<VignetteEffect>().intensity = lerpFactor * 2 - 1;
            //Camera.main.transform.position = character.transform.position
        }
        if (_endTimer > beginTextFadeTime)
        {
            var col = creditsText.color;
            col.a = Mathf.Clamp01((_endTimer - beginTextFadeTime) / (endTextFadeTime - beginTextFadeTime));
            creditsText.color = col;
        }

        if (_endTimer > closeTime) Application.Quit();
    }

    public void StartRunTimer()
    {
        if (_runTimer < 0)
            _runTimer = 0;
    }
}
