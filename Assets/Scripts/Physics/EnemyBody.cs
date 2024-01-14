using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBody : MyPhysicsBody
{
    
    public Vector2 velocity;

    public Vector3 previous_position;

    public float maxSpeed = 1.5f;

    public float maxForce = 3.0f;

    public List<EnemyBody> neighbors;
    public UnityEvent deathEffect;


    void Start()
    {
        //velocity = new Vector2(0,0);
        velocity = Vector2.zero;
        previous_position = transform.position;
        neighbors = new List<EnemyBody>();
    }

    void FixedUpdate()
    {
        previous_position = transform.position;
        if (velocity.magnitude > maxSpeed)
            {
                velocity.Normalize();
                velocity *=  maxSpeed;
            }
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
            PushToPossiblePosition(obstacle.gameObject.GetComponent<MyCollider>());
        }

        //jak enemy -> do ostatniej + event

        else if (otherBody is EnemyBody enemy)
        {
            
        }

        else if (otherBody is PlayerBody player)
        {
            deathEffect.Invoke();
        }
    }



    public void AddVelocity(Vector2 addVelocity)
    {
        velocity += addVelocity;
        if (velocity.magnitude > maxSpeed){
            velocity.Normalize();
            velocity *= maxSpeed;
        }
    }

    void Stuck()
    {
        velocity = Vector2.zero;
    }

    public override void AddNeighbor(EnemyBody neighbor)
    {
        neighbors.Add(neighbor);
    }

    public override void ResetNeighbors()
    {
        neighbors = new List<EnemyBody>();
    }

    void PushToLastPosition()
    {
        //Debug.Log("Push to previous "+gameObject);
        transform.position = previous_position;
    }
    void PushToPossiblePosition(MyCollider obstacle)
    {
        
        transform.position = obstacle.NearestPossiblePosition(transform.position, this);
    }
}
