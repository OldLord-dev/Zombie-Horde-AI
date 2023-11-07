using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MyPhysicsBody
{
    public Vector2 velocity;

    Vector3 previous_position;


    void Start()
    {
        velocity = new Vector2(0,0);
        previous_position = transform.position;
    }

    void FixedUpdate()
    {
        
        previous_position = transform.position;
        Vector3 deltaPosition = velocity*Time.deltaTime;
        transform.position += deltaPosition;
    }
    
    public override void  CollisionEffect(MyPhysicsBody otherBody)
    {
        //jak obstacle -> do ostatniej pozycji bez kolizji
        if (otherBody is StaticObstacle obstacle)
        {
            PushToLastPosition();
        }

        //jak enemy -> do ostatniej + event

        else if (otherBody is EnemyBody enemy)
        {
            PushToLastPosition();
            //jakaś utrata życia pewnie
        }
    }

    void PushToLastPosition()
    {
        transform.position = previous_position;
    }
}
