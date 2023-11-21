using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionsLoop : MonoBehaviour
{
    MyPhysicsBody[] allObstacles;
    PlayerBody onlyPlayer;
    MyPhysicsBody[] allFeelers;
    public List<EnemyBody> allEnemy;

    void Start()
    {
        //allObstacles = (StaticObstacle[])FindObjectsOfType(typeof(StaticObstacle));
        allEnemy = ((EnemyBody[])FindObjectsOfType(typeof(EnemyBody))).ToList();
        //allFeelers = ((EnemyBody[])FindObjectsOfType(typeof(EnemyBody))).ToList();;
        onlyPlayer = (PlayerBody)FindObjectOfType(typeof(PlayerBody));
        Debug.Log(allObstacles);
        Debug.Log(allEnemy);
        Debug.Log(onlyPlayer);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //sprawdzanie kolizji dla gracza
        CheckIfCollidesWithAnyObstacle(onlyPlayer);
        CheckIfCollidesWithAnyEnemy(onlyPlayer);

        //sprawdzanie kolizji dla każdego przeciwnika
        for (int i=0; i<allEnemy.Count; i++)
        {
            //czy kolizja z graczem
            CheckIfCollidesWithPlayer(allEnemy[i]);
            
            //sprawdź czy nie colliduje z statycznymi obiektami
            //Debug.Log("Checking for enemy "+i);
            CheckIfCollidesWithAnyObstacle(allEnemy[i]);


            //sprawdź czy nie koliduje z najbliższymi dynamicznymi
            CheckIfCollidesWithAnyEnemy(allEnemy[i], i);
        }
        //przejdź po wszystkich dynamicznych ciałach
    }

    void CheckIfCollidesWithAnyObstacle(MyPhysicsBody body)
    {
        foreach (MyPhysicsBody obstacle in allObstacles)
        {
            if(obstacle.c_collider.Overlaps(body.c_collider))
            {
                body.CollisionEffect(obstacle);
                //Debug.Log("Somebody " + body +" collided with " + obstacle);
            }
        }
    }

    void CheckIfCollidesWithAnyEnemy(MyPhysicsBody anybody, int index_self = -1)//index if checking enemy, to not check with itself
    {
        //no Enemy should be deleted during this loop, so no deletion during collision effect
        for (int i=0; i<allEnemy.Count; i++)
        {   
            if (index_self==-1 || index_self!=i)
            {
                EnemyBody enemy = allEnemy[i];
                if(enemy.c_collider.Overlaps(anybody.c_collider))
                {
                    anybody.CollisionEffect(enemy);
                    Debug.Log("Somebody " + anybody +" collided with " + enemy);
                }
            }
        }
    }

    void CheckIfCollidesWithPlayer(MyPhysicsBody anybody)
    {
        if(onlyPlayer.c_collider.Overlaps(anybody.c_collider))
        {
            anybody.CollisionEffect(onlyPlayer);
        }
    }

    void CheckFeelers(MyPhysicsBody body)
    {
        foreach (StaticObstacle obstacle in allObstacles)
        {
            if(obstacle.c_collider.Overlaps(body.c_collider))
            {
                body.CollisionEffect(obstacle);
                //Debug.Log("Somebody " + body +" collided with " + obstacle);
            }
        }
    }
}
