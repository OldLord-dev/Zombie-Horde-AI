using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BulletBody : MyPhysicsBody
{
    public float force;
    public Vector2 velocity;
    private Vector3 mousePos;
    private Camera mainCam;
    public float maxSpeed = 1.5f;
    public CollisionsLoop collisionsLoop;
    private void OnEnable()
    {
        collisionsLoop = (CollisionsLoop)FindObjectOfType(typeof(CollisionsLoop));
        collisionsLoop.bullets.Add(this);
        StartCoroutine("Wait2Seconds");

    }
    void FixedUpdate()
    {
        //if (velocity.magnitude > maxSpeed)
        //{
        //    velocity.Normalize();
        //    velocity *= maxSpeed;
        //}
        Vector3 deltaPosition = velocity * Time.deltaTime;
        transform.position += deltaPosition;
    }

    public override void  CollisionEffect(MyPhysicsBody otherBody)
    {
        //jak obstacle -> do ostatniej pozycji bez kolizji
        if (otherBody is StaticObstacle obstacle)
        {
            collisionsLoop.SetToDestroy(gameObject);
        }

        //jak enemy -> do ostatniej + event

        else if (otherBody is EnemyBody enemy)
        {
            collisionsLoop.SetToDestroy(enemy.gameObject);
            collisionsLoop.SetToDestroy(gameObject);
        }
    }

    IEnumerator Wait2Seconds()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        collisionsLoop.bullets.Remove(this);
    }
}
