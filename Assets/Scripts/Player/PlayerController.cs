using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
    {
Rigidbody2D body;

float horizontal;
float vertical;

public float runForce = 20.0f;

void Start ()
{
   body = GetComponent<Rigidbody2D>(); 
}

void Update ()
{
   horizontal = Input.GetAxisRaw("Horizontal");
   vertical = Input.GetAxisRaw("Vertical"); 
}

private void FixedUpdate()
{  
   body.AddForce(new Vector2(horizontal * runForce, vertical * runForce));
}
}
