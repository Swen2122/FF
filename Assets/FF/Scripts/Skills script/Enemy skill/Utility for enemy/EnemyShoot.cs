using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public Transform shootPoint;
    
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float spreadAngle = 5f;
    [SerializeField] private int projectilesCount = 1;
    [SerializeField] private float angleBetweenProjectiles = 10f;
    
    public void Bullet(GameObject bulletPrefab, Transform target, float speed, int damage, float lifetime)
    {
        if (bulletPrefab == null || shootPoint == null || target == null)
        {
            Debug.LogWarning("Missing required references for shooting!");
            return;
        }
    }
}
