using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public float runForce = 20.0f;
    Rigidbody2D corpse;

    public Vector2 placeholderDirection;
    void Start ()
    {
        corpse = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {  
        corpse.AddForce(placeholderDirection*runForce);
    }
}
