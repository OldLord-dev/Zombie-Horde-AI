using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    SteeringBehaviors steeringBehaviors;
    public float speed = 2.0f;
    EnemyBody corpse;

    public Vector2 placeholderDirection;
    void Start ()
    {
        corpse = GetComponent<EnemyBody>();
        steeringBehaviors = GetComponent<SteeringBehaviors>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {  
        corpse.velocity = steeringBehaviors.Wander() * speed;
    }
}
