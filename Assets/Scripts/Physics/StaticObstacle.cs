using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ObstacleType 
{
    Wall, Circle
}
public class StaticObstacle : MyPhysicsBody
{
    public ObstacleType type; //set in Unity
}
