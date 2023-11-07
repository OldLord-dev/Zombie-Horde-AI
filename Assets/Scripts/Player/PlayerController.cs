using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerBody playerBody;

    float horizontal;
    float vertical;

    public float speed = 2.0f;

    void Start ()
    {
        playerBody = GetComponent<PlayerBody>(); 
    }

    void Update ()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical"); 
    }

    private void FixedUpdate()
    {  
        playerBody.velocity = new Vector2(horizontal * speed, vertical * speed);
    }
}
