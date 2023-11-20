using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCollider: MyCollider
{
    public float c_radius { get; set; }

    public void Start()
    {
        c_radius = transform.localScale.x/2;
        Debug.Log(c_radius);
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

}
