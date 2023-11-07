using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPhysicsBody : MonoBehaviour
{
    
    public MyCollider c_collider;
    
    void Awake ()
    {
        c_collider = GetComponent<MyCollider>();
        Debug.Log("Collider added! " + c_collider);
    }

    public virtual void CollisionEffect(MyPhysicsBody other)
    {
        return ;
    }


}

