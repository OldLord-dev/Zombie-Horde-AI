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
    private void OnEnable()
    {
        StartCoroutine("Wait2Seconds");
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.localPosition - mousePos;
        velocity = new Vector2(direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(rotation.x, rotation.y) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, rot + 90);
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
            gameObject.SetActive(false);
        }

        //jak enemy -> do ostatniej + event

        else if (otherBody is EnemyBody enemy)
        {
            enemy.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    IEnumerator Wait2Seconds()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
