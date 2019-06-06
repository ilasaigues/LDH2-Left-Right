using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyEntity : MonoBehaviour, ICollidable
{

    public KeyData data;
    public float hoverPower = .2f;
    public float hoverRate = 1;
    public SoundValue keyPickupSound;

    public static System.Action<KeyData> OnKeyPickup = (kd) => { };

    Vector3 startLocalPos;
    private void Awake()
    {
        startLocalPos = transform.localPosition;
    }


    // Start is called before the first frame update
    void Start()
    {
        OnKeyPickup += KeyPickedUp;
        GetComponent<SpriteRenderer>().color = data.color;
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

    private void OnDrawGizmos()
    {
        Start();
    }

    public void CollidedWithCharacterController(CharacterController characterController)
    {
        Director.GetManager<SoundManager>().PlaySound(keyPickupSound);
        characterController.AddKey(data);
        OnKeyPickup(data);
        OnKeyPickup -= KeyPickedUp;
    }
}
