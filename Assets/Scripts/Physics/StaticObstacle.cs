using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObstacle : MyPhysicsBody
{
    public float distanceFromBoundary = 3.0f;


    public (bool, Vector2) GetHidingPosition(Vector2 playerPos)
    {
        if (c_collider is CircleCollider circleCollider)
        {
            float distanceFromCentre = circleCollider.c_radius +  distanceFromBoundary;
            Vector2 centre = transform.position;
            Vector2 hidingPoint = ( centre - playerPos).normalized * distanceFromCentre +centre;
            return (true, hidingPoint);
        }
        else return (false, Vector2.positiveInfinity);
    }
}
