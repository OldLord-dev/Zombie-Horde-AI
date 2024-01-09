using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CircleCollider: MyCollider
{
    public float c_radius { get; set; }

    public void Awake()
    {
        c_radius = transform.localScale.x/2;
        //Debug.Log(c_radius);
    }

    public override bool Overlaps(MyCollider other)
    {
        if (other is CircleCollider circleCollider)
        {
            return DoesCollideWithCircle(circleCollider);
        }
        else if (other is RectangleCollider rectangleCollider)
        {
            return rectangleCollider.Overlaps(this);
        }
        return false;
    }

    private bool DoesCollideWithCircle(CircleCollider circleCollider)
    {
        float sumOfRadius = c_radius + circleCollider.c_radius;
        float distance = Vector2.Distance(transform.position, circleCollider.transform.position);
        return distance <= sumOfRadius;
    }

    public (bool, Vector2, float) IntersectingPoints(Vector2 startpoint, Vector2 endpoint, float expand)
    {
        float dx, dy, A, B, C, det, t;

        float radius = c_radius+expand;
        Vector2 centre = transform.position;
        dx = endpoint.x - startpoint.x;
        dy = endpoint.y - startpoint.y;

        A = dx * dx + dy * dy;
        B = 2 * (dx * (startpoint.x - centre.x) + dy * (startpoint.y - centre.y));
        C = (startpoint.x - centre.x) * (startpoint.x - centre.x) + (startpoint.y - centre.y) * (startpoint.y - centre.y) - radius * radius;

        det = B * B - 4 * A * C;
        if ((A <= 0.0000001) || (det < 0))
        {
            return (false, Vector2.positiveInfinity,  Mathf.Infinity);
        }
        else if (det == 0)
        {
            // One solution.
            t = -B / (2 * A);
            Vector2 intersection1 = new Vector2(startpoint.x + t * dx, startpoint.y + t * dy);
            float distance = (new Vector2(t * dx,t * dy)).magnitude;
            //Debug.DrawLine( startpoint,  intersection1);
            return (true, intersection1,distance);
        }
        else
        {
            // Two solutions.
            t = (float)((-B + Mathf.Sqrt(det)) / (2 * A));
            Vector2 intersection1 = new Vector2(startpoint.x + t * dx, startpoint.y + t * dy);
            //Debug.DrawLine( startpoint,  intersection1);
            float distance1 = (new Vector2(t * dx,t * dy)).magnitude;
            t = (float)((-B - Mathf.Sqrt(det)) / (2 * A));
            Vector2 intersection2 = new Vector2(startpoint.x + t * dx, startpoint.y + t * dy);
            //Debug.DrawLine( startpoint,  intersection2);
            float distance2 = (new Vector2(t * dx,t * dy)).magnitude;
            //select closer
            if (distance1 < distance2 )
            {
                return (true, intersection1, distance1);
            }

            else return (true, intersection2, distance2);
        }
    }

}
