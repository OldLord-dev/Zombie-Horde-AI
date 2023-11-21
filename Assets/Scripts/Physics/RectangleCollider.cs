using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleCollider: MyCollider
{
    public Vector2 c_size { get; set; }

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
        if (vertices.Length == 1) return minProj;
        
        for (int i = 1; i < vertices.Length; i++)
        {
            float proj = Vector2.Dot(vertices[i], axis);
            if (proj < minProj)
            {
                minProj = proj;
            }
        }
        return minProj;
    }

    public (bool, Vector2) IntersectingSide(Vector2 startpoint, Vector2 endpoint)
    {
        Vector2[] corners = new Vector2[4];

        // Get the local coordinates of the rectangle corners
        Vector2[] localCorners = new Vector2[]
        {
            new Vector2(-c_size.x / 2, -c_size.y / 2),
            new Vector2(c_size.x / 2, -c_size.y / 2),
            new Vector2(c_size.x / 2, c_size.y / 2),
            new Vector2(-c_size.x / 2, c_size.y / 2)
        };

        // Transform local coordinates to world coordinates
        for (int i = 0; i < 4; i++)
        {
            corners[i] = transform.TransformPoint(localCorners[i]);
        }

        for (int i = 0; i < 4; i++) //loop rectangle sides
        {   
            Vector2 p1 = corners[i];
            Vector2 p2;
            if (i==3) p2 = corners[0];
            else p2 = corners[i+1];

            if (AreSegmentsIntersecting(p1, p2, startpoint, endpoint))
            {
                Vector2[] side = {p1,p2};
                return (true, ClosestSidePoint(endpoint, side));
            }
        }
        return (false, Vector2.positiveInfinity);
    }

    public static bool AreSegmentsIntersecting(Vector2 A, Vector2 B, Vector2 startpoint, Vector2 endpoint)
    {
        // Calculate the direction vectors of the two line segments
        Vector2 AB = B - A;
        Vector2 AC = startpoint - A;
        Vector2 AD = endpoint - A;

        Vector2 CD = endpoint - startpoint;
        Vector2 CA = A - startpoint;
        Vector2 CB = B - startpoint;

        // Check for intersection using cross products
        float crossProductABAC = Vector3.Cross(AB, AC).z;
        float crossProductABAD = Vector3.Cross(AB, AD).z;

        float crossProductCDBC = Vector3.Cross(CD, CB).z;
        float crossProductCDCA = Vector3.Cross(CD, CA).z;

        // If cross products have different signs, the segments intersect
        return (crossProductABAC * crossProductABAD < 0) && (crossProductCDBC * crossProductCDCA < 0);
    }


    public Vector2 ClosestSidePoint(Vector2 point, Vector2[] side)
    {
        Vector2 AB = side[1] - side[0];
        Vector2 AP = point - side[0];

        float t = Vector2.Dot(AP, AB) / Vector2.Dot(AB, AB);

        // Calculate the nearest point on segment AB to point D
        Vector2 nearestPoint = side[0] + t * AB;
        return nearestPoint;
    }

}

