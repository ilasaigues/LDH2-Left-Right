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
    float timer = -1;
    CharacterController character;
    public UnityEngine.UI.Text creditsText;
    public AudioSource partyHornSound;
    public AudioSource ominousSound;
    void Start()
    {
        character = FindObjectOfType<CharacterController>();
    }

    public void CollidedWithCharacterController(CharacterController characterController)
    {
        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponent<ParticleSystem>());
        Instantiate(explosion, transform.position, Quaternion.identity);
        Instantiate(partyHornSound);
        ominousSound = Instantiate(ominousSound);
        timer = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterController charCon = collision.GetComponent<CharacterController>();
        if (charCon != null)
        {
            CollidedWithCharacterController(charCon);
        }
    }

    void FixedUpdate()
    {
        if (timer >= 0)
        {
            timer += Time.deltaTime;
        }
        if (timer > implosionBeginTime)
        {
            float lerpFactor = Mathf.Clamp01((timer - implosionBeginTime) / (implosionCollapseTime - implosionBeginTime));
            character.Lock();
            character.transform.position = Vector3.Lerp(character.transform.position, transform.position, lerpFactor * lerpFactor);

            Camera.main.GetComponent<SmoothCameraFollow>().verticalLock = true;
            Camera.main.GetComponent<SmoothCameraFollow>().lerp = .1f;
            ominousSound.volume = lerpFactor;
            Camera.main.GetComponent<VignetteEffect>().intensity = lerpFactor * 2 - 1;
            //Camera.main.transform.position = character.transform.position
        }
        if (timer > beginTextFadeTime)
        {
            var col = creditsText.color;
            col.a = Mathf.Clamp01((timer - beginTextFadeTime) / (endTextFadeTime - beginTextFadeTime));
            creditsText.color = col;
        }

        if (timer > closeTime) Application.Quit();

    }
}
