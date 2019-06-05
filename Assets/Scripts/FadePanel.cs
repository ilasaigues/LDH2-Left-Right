using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePanel : MonoBehaviour
{
    public float minimumDistance;
    public float maximumDistance;

    CharacterController character;
    SpriteRenderer[] renderers;

    // Start is called before the first frame update
    void Start()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        character = FindObjectOfType<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var renderer in renderers)
        {
            var col = renderer.color;
            col.a = 1 - Mathf.Clamp01((Vector3.Distance(character.transform.position, transform.position) - minimumDistance) / (maximumDistance - minimumDistance));
            renderer.color = col;
        }
    }
}
