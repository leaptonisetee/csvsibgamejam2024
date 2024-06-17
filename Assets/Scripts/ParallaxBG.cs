using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBG : MonoBehaviour
{
    private float startPos;

    public Transform cam;
    public float parallaxEffect;

    void Start()
    {
        startPos = transform.position.x;
    }

    void FixedUpdate()
    {
        float dist = cam.position.x * parallaxEffect;

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
    }
}
