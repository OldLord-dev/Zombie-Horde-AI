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
            if(obstacle.type == ObstacleType.Wall && obstacle.c_collider is RectangleCollider wall_collider)
            {
                (detected, closestPoint) = wall_collider.IntersectingSide(transform.parent.position, transform.position);
                if (!detected)
                {
                    NoCollision();
                    //Vector2 penetration = closestPoint - transform.position;
                    //overshoot = penetration.magnitude;
                }
                //policzyć Vector2 closestPoint (najbliższy feelerowi)
                //overshoot = długość(transfomr.position - closestPoint)*normalizacja;
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
