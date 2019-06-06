using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPowerup : MonoBehaviour, ICollidable
{
    public SoundValue powerupPickupSound;
    public void CollidedWithCharacterController(CharacterController characterController)
    {
        characterController.maxJumps = 2;
        Director.GetManager<SoundManager>().PlaySound(powerupPickupSound);
        Destroy(gameObject);
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
