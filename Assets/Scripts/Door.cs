using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, ICollidable
{

    public KeyData key;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().color = key.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CollidedWithCharacterController(CharacterController characterController)
    {
        if (characterController.CanOpenDoor(this))
        {
            Debug.Log("Door open");
            Destroy(gameObject);
        }
    }
}
