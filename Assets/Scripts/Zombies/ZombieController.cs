using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public float speed = 2.0f;
    EnemyBody corpse;

    public Vector2 placeholderDirection;
    void Start ()
    {
        corpse = GetComponent<EnemyBody>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {  
        corpse.velocity = placeholderDirection*speed;
    }
}
