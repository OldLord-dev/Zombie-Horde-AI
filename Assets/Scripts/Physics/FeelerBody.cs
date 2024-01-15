using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeelerBody : MyPhysicsBody
{   
    //public StaticObstacle closeWall;
    public bool detected;
    public Vector2 closestPoint;//najbliższy punkty feelera do ściany!

    private float lengthOffset = 1.0f;
    private float lengthModifier = 2.0f;
    public CollisionsLoop collisionsLoop;
    private void OnEnable()
    {
        collisionsLoop = (CollisionsLoop)FindObjectOfType(typeof(CollisionsLoop));
        collisionsLoop.allFeelers.Add(this);
    }

    private void OnDisable()
    {
        collisionsLoop.allFeelers.Remove(this);
    }
    void Start()
    {
        NoCollision();
    }

    public override void  CollisionEffect(MyPhysicsBody otherBody)
    {
        if (otherBody is StaticObstacle obstacle)
        {
            if(c_collider is PointCollider && obstacle.c_collider is RectangleCollider rectCollider)
            {
                
                (detected, closestPoint) = rectCollider.IntersectingSide(transform.parent.position, transform.position);
                //Debug.Log("collided with wall detected = "+detected+" closestPoint = "+closestPoint.x+","+closestPoint.y);
                
            }
        }
    }

    public override void NoCollision()
    {
        detected = false;
        closestPoint = Vector2.positiveInfinity;
    }

}
