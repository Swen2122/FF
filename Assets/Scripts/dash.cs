using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dash : MonoBehaviour
{
    Rigidbody2D body;

    public float dash_force = 10f;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Shift"))
        {
            Vector3 dash_cord = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            dash_cord.Normalize();
            body.AddForce(dash_cord * dash_force, ForceMode2D.Impulse);
            Debug.Log("де ривок сука?");
        }

    }
}
