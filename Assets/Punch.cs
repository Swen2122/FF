using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    public Damage weapon;
    GameObject[] enemy;
    public float attackRadius = 5f;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { 
            weapon.atk(enemy, 15); 
        }
    }
}
