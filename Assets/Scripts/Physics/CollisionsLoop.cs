using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionsLoop : MonoBehaviour
{
    public StaticObstacle[] allObstacles;
    public PlayerBody onlyPlayer;
    public List<FeelerBody> allFeelers;
    public List<DetectionBody> allDetectors;
    public List<EnemyBody> allEnemy;
    public float neighborDistance = 3f;

    void Start()
    {
        allObstacles = (StaticObstacle[])FindObjectsOfType(typeof(StaticObstacle));
        allEnemy = ((EnemyBody[])FindObjectsOfType(typeof(EnemyBody))).ToList();
        allFeelers = ((FeelerBody[])FindObjectsOfType(typeof(FeelerBody))).ToList();;
        allDetectors = ((DetectionBody[])FindObjectsOfType(typeof(DetectionBody))).ToList();;
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
        CheckIfCollidesWithOrNearAnyEnemy(onlyPlayer);

        //sprawdzanie kolizji dla każdego przeciwnika
        for (int i=0; i<allEnemy.Count; i++)
        {
            //czy kolizja z graczem
            CheckIfCollidesWithPlayer(allEnemy[i]);
            
            //sprawdź czy nie colliduje z statycznymi obiektami
            //Debug.Log("Checking for enemy "+i);
            CheckIfCollidesWithAnyObstacle(allEnemy[i]);


            //sprawdź czy nie koliduje z najbliższymi dynamicznymi

            allEnemy[i].ResetNeighbors();
            CheckIfCollidesWithOrNearAnyEnemy(allEnemy[i], i);
        }
        //przejdź po wszystkich dynamicznych ciałach
        CheckAllDetections();
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

    void CheckIfCollidesWithOrNearAnyEnemy(MyPhysicsBody anybody, int index_self = -1)//index if checking enemy, to not check with itself
    {
        //no Enemy should be deleted during this loop, so no deletion during collision effect

        
        for (int i=0; i<allEnemy.Count; i++)
        {   
            if (index_self==-1 || index_self!=i)
            {
                EnemyBody enemy = allEnemy[i];
                float distanceBetween = (anybody.transform.position - enemy.transform.position).magnitude;
                if(distanceBetween < neighborDistance)
                {
                    anybody.AddNeighbor(enemy);
                    if(enemy.c_collider.Overlaps(anybody.c_collider))
                    {
                        anybody.CollisionEffect(enemy);
                        //Debug.Log("Somebody " + anybody +" collided with " + enemy);
                    }
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

    void CheckAllDetections()
    {   
        //Debug.Log("CheckAllFeelers");
        foreach (FeelerBody feeler in allFeelers)
        {
            CheckIfCollidesWithAnyObstacle(feeler);
            //Debug.DrawLine(feeler.transform.position, feeler.transform.parent.position, Color.red);
        }

        foreach (DetectionBody feeler in allDetectors)
        {
            feeler.Recaliber();
            CheckIfCollidesWithAnyObstacle(feeler);
        }
    }


    void TagAllEnemyNeighbors()
    {

    }
}
