using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCollider : MyCollider
{
    //start position - transform.parent.position 
    //end position - transfrom.position
    
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
        //Compare distance to radius:
        Vector2 thisPosition2D = transform.position;
        Vector2 otherCenter = circleCollider.transform.position;
        float circleRadius = circleCollider.c_radius;
        float distance = Vector2.Distance(thisPosition2D, otherCenter);

        return distance <= circleRadius;
    }

    private bool DoesCollideWithRectangle(RectangleCollider rectangleCollider)
    {
        //Rotate point:
        Vector2 thisPosition2D = transform.position;

        
        Vector2 otherPosition2D = rectangleCollider.transform.position;
        Vector2 otherMin = otherPosition2D - rectangleCollider.c_size / 2;
        Vector2 otherMax = otherPosition2D + rectangleCollider.c_size / 2;
        //Debug.Log("Checking collision of point "+thisPosition2D.x+","+thisPosition2D.y+" with rectangle of min: "+otherMin+" ,max: "+otherMax);
        // Rotate the point's position in the opposite direction to the box's rotation
        float angle = rectangleCollider.transform.rotation.eulerAngles.z;
        Quaternion inverseRotation = Quaternion.Euler(0, 0, -angle);
        Vector2 rotatedPoint = inverseRotation * (thisPosition2D - otherPosition2D);
        rotatedPoint += otherPosition2D;
        
        return rotatedPoint.x >= otherMin.x && rotatedPoint.x <= otherMax.x &&
               rotatedPoint.y >= otherMin.y && rotatedPoint.y <= otherMax.y;
    }


}
