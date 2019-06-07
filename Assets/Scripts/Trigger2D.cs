using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Trigger2D : MonoBehaviour
{
    [SerializeField]
    public UnityEvent OnTriggerEnter2DEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter2DEvent.Invoke();
    }

}
