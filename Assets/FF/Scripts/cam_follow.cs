using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam_follow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 0, -10);
    public float smoothTime = 0.25f;

    Vector3 currentVelocity;
    void Start()
    {
        player = PlayerUtility.PlayerTransform;
    }
    private void LateUpdate()
    {
        if(PauseManager.IsPaused) return;
        transform.position = Vector3.SmoothDamp(transform.position, player.position + offset, ref currentVelocity, smoothTime);
    }

}
