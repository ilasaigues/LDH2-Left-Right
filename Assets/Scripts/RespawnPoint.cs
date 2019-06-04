using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour, ICollidable
{
    public void CollidedWithCharacterController(CharacterController characterController)
    {
        characterController.SetRespawnPoint(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterController charCon = collision.GetComponent<CharacterController>();
        if (charCon != null)
        {
            CollidedWithCharacterController(charCon);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CharacterController charCon = collision.GetComponent<CharacterController>();
        if (charCon != null)
        {
            CollidedWithCharacterController(charCon);
        }
    }
}
