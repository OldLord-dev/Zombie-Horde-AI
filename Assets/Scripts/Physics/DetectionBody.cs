using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionBody : MyPhysicsBody
{   
    //public StaticObstacle closeWall;
    public bool detected;
    public Vector2 closestIntersection;//najbliższy punkty feelera do ściany!
    public float distance;

    public float obstacleRadius;
    public float distanceCentre;

    public Vector2 lateralForce;

    private float lengthOffset = 2.0f;

    void Start()
    {
        NoCollision();
    }

    public override void  CollisionEffect(MyPhysicsBody otherBody)
    {
        if (otherBody is StaticObstacle obstacle)
        {
            if (c_collider is RectangleCollider thisCollider && obstacle.c_collider is CircleCollider circleCollider)
            {
                Vector3 startpoint = transform.parent.position;
                Vector3 endpoint = (transform.position - startpoint) *2 + startpoint;
                float potentialDistance;
                Vector2 potentialClosestPoint;
                (detected, potentialClosestPoint,potentialDistance) = circleCollider.IntersectingPoints(startpoint, endpoint, thisCollider.c_size.x/2);
                
                if (detected)
                {
                    if(potentialDistance < distance)
                    {
                        closestIntersection = potentialClosestPoint;
                        distance = potentialDistance;
                        obstacleRadius = circleCollider.c_radius;
                        Vector2 circleCentre = circleCollider.transform.position;
                        Vector2 extendedEndpoint = (endpoint-startpoint)*10.0f+startpoint;
                        Vector2 wouldBeIntersection = thisCollider.ClosestSidePoint(circleCentre, new Vector2[]{startpoint,extendedEndpoint});
                        //distanceCentreX = (wouldBeIntersection- new Vector2(startpoint.x, startpoint.y)).magnitude;
                        distanceCentre = (new Vector2(startpoint.x, startpoint.y) - circleCentre).magnitude;
                        Vector2 lateralVector = (wouldBeIntersection-circleCentre);
                    
                        lateralForce = lateralVector.normalized * (thisCollider.c_size.x/2 + circleCollider.c_radius - lateralVector.magnitude );
                    }
                }
            }
        }
    }

    public override void NoCollision()
    {
        detected = false;
        closestIntersection = Vector2.positiveInfinity;
        distance =  Mathf.Infinity;
        lateralForce = Vector2.positiveInfinity;
    }

    public void Recaliber()
    {
        if (c_collider is RectangleCollider rectCollider)
        {   
            EnemyBody enemyBody = GetComponentInParent<EnemyBody>();
            Vector2 currentVelocity = enemyBody.velocity;
            float newLength = currentVelocity.magnitude/enemyBody.maxSpeed *lengthOffset + lengthOffset;
            rectCollider.Resize(1,newLength);
            rectCollider.transform.localPosition = new Vector3(0,-newLength/2,0);
        }    
    }
}

