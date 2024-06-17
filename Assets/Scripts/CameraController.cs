using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Player;

    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    private void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, Player.transform.position + offset, ref velocity, 0.15f);
    }
}
