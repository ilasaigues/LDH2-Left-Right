using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [Range(0, 1)]
    public float lerp;
    public Transform target;
    public Vector3 followOffset;
    public float verticalOffsetPower;
    public bool verticalLock = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 newPos = target.transform.position;

            if (!verticalLock)
            {
                float height = (Mathf.FloorToInt((target.position.y + .5f) / LevelChunk.size.y) * LevelChunk.size.y);
                float heightOffset = (LevelChunk.size.y - 1) / 2f;

                Vector2 cameraPixelSize = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);

                Vector2 screenCenter = cameraPixelSize / 2;


                Vector3 verticaloffset = new Vector3(0, Input.GetAxis("Vertical"), 0);

                newPos.y = height + heightOffset;
                newPos += verticaloffset * verticalOffsetPower;
            }

            transform.position = Vector3.Lerp(transform.position, newPos + followOffset, lerp);
        }
    }
}
