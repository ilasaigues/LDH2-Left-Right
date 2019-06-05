using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPowerup : MonoBehaviour, ICollidable
{
    public AudioSource pickupSound;
    public void CollidedWithCharacterController(CharacterController characterController)
    {
        characterController.maxJumps = 2;
        Destroy(gameObject);
        Destroy(Instantiate(pickupSound).gameObject, pickupSound.clip.length);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterController charCon = collision.GetComponent<CharacterController>();
        if (charCon != null)
        {
            CollidedWithCharacterController(charCon);
        }
    }
}
