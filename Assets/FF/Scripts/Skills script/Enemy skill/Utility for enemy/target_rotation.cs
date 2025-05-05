using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target_rotation : MonoBehaviour
{
    public Transform player; // ֳ��, ���� � �������
    void Start()
    {
        player = PlayerUtility.PlayerTransform;
    }
    void Update()
    {
        Vector3 playes_position = player.position - transform.position;
        playes_position.Normalize();

        float rotZ = Mathf.Atan2(playes_position.y, playes_position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ); ; ;
    }
}
