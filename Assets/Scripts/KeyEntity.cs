using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyEntity : MonoBehaviour
{

    public KeyData data;
    public float hoverPower = .2f;
    public float hoverRate = 1;
    public AudioSource pickupSoundSource;

    public static System.Action<KeyData> OnKeyPickup = (kd) => { };


    Vector3 startLocalPos;
    // Start is called before the first frame update
    void Start()
    {
        OnKeyPickup += KeyPickedUp;
        GetComponent<SpriteRenderer>().color = data.color;
        startLocalPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = startLocalPos + Vector3.up * Mathf.Sin(Time.time * hoverRate) * hoverPower;
    }

    void KeyPickedUp(KeyData data)
    {
        if (this.data == data) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterController character = collision.gameObject.GetComponent<CharacterController>();
        if (character)
        {
            Destroy(Instantiate(pickupSoundSource), pickupSoundSource.clip.length);
            character.AddKey(data);
            OnKeyPickup(data);
            OnKeyPickup -= KeyPickedUp;
        }
    }

    private void OnDrawGizmos()
    {
        Start();
    }
}
