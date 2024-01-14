using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCollider : MonoBehaviour
{
    public virtual bool Overlaps(MyCollider other)
    {
        return false;
    }

    public virtual Vector2 NearestPossiblePosition(Vector2 wannabePosition, EnemyBody agent)
    {
        return Vector2.zero;
    }
}
