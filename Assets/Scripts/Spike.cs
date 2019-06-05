using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour, ICollidable
{
    public void CollidedWithCharacterController(CharacterController characterController)
    {
        characterController.Kill();
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
