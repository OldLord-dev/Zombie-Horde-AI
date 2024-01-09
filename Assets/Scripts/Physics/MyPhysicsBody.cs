using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPhysicsBody : MonoBehaviour
{
    
    public MyCollider c_collider;
    
    void Awake ()
    {
        c_collider = GetComponent<MyCollider>();
    }

    public virtual void CollisionEffect(MyPhysicsBody body)
    {
        return ;
    }

    public virtual void NoCollision() {}

    public virtual void AddNeighbor(EnemyBody neighbor) {}

    public virtual void ResetNeighbors() {}


}

