using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCollider : MonoBehaviour
{
    public virtual bool Overlaps(MyCollider other)
    {
        return false;
    }
}
