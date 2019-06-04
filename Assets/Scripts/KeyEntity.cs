using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyEntity : MonoBehaviour
{
    public KeyData data;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().color = data.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterController character = collision.gameObject.GetComponent<CharacterController>();
        if (character)
        {
            character.AddKey(data);
            Destroy(gameObject);
        }
    }
}
