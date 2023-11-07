using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleCollider: MyCollider
{
    public Vector2 c_size { get; set; }
    public float c_rotation { get; set; }

    public void Start()
    {
        c_size = new Vector2(transform.localScale.x, transform.localScale.y);
    }

    public override bool Overlaps(MyCollider other)
    {
        if (other is CircleCollider circleCollider)
        {
            return DoesCollideWithCircle(circleCollider);
        }
        else if (other is RectangleCollider rectangleCollider)
        {
            return DoesCollideWithRectangle(rectangleCollider);
        }
        return false;
    }

    private bool DoesCollideWithCircle(CircleCollider circleCollider)
    {
        // Implement collision detection logic for rotated box vs circle
        // You can use the position and size of the box, the circle's position, and radius
        Vector2 thisPosition2D = transform.position;
        Vector2 thisMin = thisPosition2D - c_size / 2;
        Vector2 thisMax = thisPosition2D + c_size / 2;
        
        Vector2 otherCenter = circleCollider.transform.position;
        float circleRadius = circleCollider.c_radius;
        float angle = transform.rotation.eulerAngles.z;
        Quaternion inverseRotation = Quaternion.Euler(0, 0, -angle);

        // Rotate the circle's position in the opposite direction to the box's rotation
        Vector2 rotatedCircleCenter = inverseRotation * (otherCenter - thisPosition2D);
        rotatedCircleCenter += thisPosition2D;
        float closestX = Mathf.Clamp(rotatedCircleCenter.x, thisMin.x, thisMax.x);
        float closestY = Mathf.Clamp(rotatedCircleCenter.y, thisMin.y, thisMax.y);

        float distance = Vector2.Distance(rotatedCircleCenter, new Vector2(closestX, closestY));

        return distance <= circleRadius;
    }

    private bool DoesCollideWithRectangle(RectangleCollider rectangleCollider)
    {
        
        Vector2 thisPosition2D = transform.position;
        Vector2 thisMin = thisPosition2D - c_size / 2;
        Vector2 thisMax = thisPosition2D + c_size / 2;
        Vector2[] thisVertices = GetVertices(thisMin, thisMax, transform.rotation.eulerAngles.z);

        Vector2 otherPosition2D = rectangleCollider.transform.position;
        Vector2 otherMin = otherPosition2D - rectangleCollider.c_size / 2;
        Vector2 otherMax = otherPosition2D + rectangleCollider.c_size / 2;
        Vector2[] otherVertices = GetVertices(otherMin, otherMax, rectangleCollider.transform.rotation.eulerAngles.z);
        
        

        // Check for overlap along each axis formed by edges of both boxes
        for (int axisIndex = 0; axisIndex < 2; axisIndex++)
        {
            // Calculate the axis perpendicular to the current edge
            Vector2 axis = thisVertices[axisIndex + 2] - thisVertices[axisIndex];
            axis = new Vector2(-axis.y, axis.x);
            axis.Normalize();

            // Project the vertices of both boxes onto the axis
            float thisMinProj = Project(thisVertices, axis);
            float thisMaxProj = thisMinProj + Vector2.Dot(axis, thisVertices[2] - thisVertices[0]);

            float otherMinProj = Project(otherVertices, axis);
            float otherMaxProj = otherMinProj + Vector2.Dot(axis, otherVertices[2] - otherVertices[0]);

            // Check for overlap
            if (thisMaxProj < otherMinProj || thisMinProj > otherMaxProj)
            {
                // Separating axis found, no collision
                return false;
            }
        }

        return true;
    }


    private Vector2[] GetVertices(Vector2 min, Vector2 max, float angle)
    {
        Vector2[] vertices = new Vector2[4];

        float halfWidth = (max.x - min.x) / 2;
        float halfHeight = (max.y - min.y) / 2;
        Vector2 center = min + new Vector2(halfWidth, halfHeight);

        vertices[0] = Quaternion.Euler(0, 0, angle) * new Vector2(-halfWidth, -halfHeight);
        vertices[1] = Quaternion.Euler(0, 0, angle) * new Vector2(halfWidth, -halfHeight);
        vertices[2] = Quaternion.Euler(0, 0, angle) * new Vector2(halfWidth, halfHeight);
        vertices[3] = Quaternion.Euler(0, 0, angle) * new Vector2(-halfWidth, halfHeight);

        for (int i=0; i<4; i++) vertices[i] = vertices[i]+center;

        return vertices;
    }

    private float Project(Vector2[] vertices, Vector2 axis)
    {
        float minProj = Vector2.Dot(vertices[0], axis);
        for (int i = 1; i < 4; i++)
        {
            float proj = Vector2.Dot(vertices[i], axis);
            if (proj < minProj)
            {
                minProj = proj;
            }
        }
        return minProj;
    }

}