using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeelerBody : MyPhysicsBody
{   
    //public StaticObstacle closeWall;
    public bool detected;
    public Vector2 closestPoint;//najbliższy punkty feelera do ściany!

    public override void  CollisionEffect(MyPhysicsBody otherBody)
    {
        if (otherBody is StaticObstacle obstacle)
        {
            if(obstacle.c_collider is RectangleCollider rectCollider)
            {
                (detected, closestPoint) = rectCollider.IntersectingSide(transform.parent.position, transform.position);
                if (!detected)
                {
                    NoCollision();
                    //Vector2 penetration = closestPoint - transform.position;
                    //overshoot = penetration.magnitude;
                }
                //policzyć Vector2 closestPoint (najbliższy feelerowi)
                //overshoot = długość(transfomr.position - closestPoint)*normalizacja;
            }
            else if (obstacle.c_collider is CircleCollider circleCollider)
            {
                //
            }
        }
    }

    public void NoCollision()
    {
        //closeWall = null;
        //distanceToCloseWall = float.MaxValue;
        //overshoot = 0;
    }
}
