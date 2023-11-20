using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBody : MyPhysicsBody
{
    
    public Vector2 velocity;

    public Vector3 previous_position;


    void Start()
    {
        velocity = new Vector2(0,0);
        previous_position = transform.position;
    }

    void FixedUpdate()
    {
        previous_position = transform.position;
        Vector3 deltaPosition = velocity*Time.deltaTime;

        //transform.LookAt(transform.position + deltaPosition);
        
        transform.position += deltaPosition;
        if (velocity != Vector2.zero) 
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg + 90.0f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }


        //ustaw kąt
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
            //idk, czy tu się przepychają? czy mogą na siebie nachodzić
        }

        else if (otherBody is PlayerBody player)
        {
            PushToLastPosition();
            //idk, czy tu się przepychają? czy mogą na siebie nachodzić
        }
    }

    void PushToLastPosition()
    {
        //Debug.Log("Push to previous "+gameObject);
        transform.position = previous_position;
    }

    void ChangeVelocity()
    {
        
    }
}
