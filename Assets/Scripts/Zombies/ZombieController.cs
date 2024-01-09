using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    SteeringBehaviors steeringBehaviors;
    EnemyBody corpse;
    public float mass;
    public Vector2 baseForce;

    public Transform transformPoint;//robocze;

    public Vector2 placeholderDirection;

    public SimulationPhase currentPhase = SimulationPhase.Hiding;

    public int minGroup = 4;
    void Start ()
    {
        corpse = GetComponent<EnemyBody>();
        steeringBehaviors = GetComponent<SteeringBehaviors>();
        transformPoint = GameObject.Find("Target").transform;
    }

    // Update is called once per frame
    void Update()
    {
        baseForce = transformPoint.position - transform.position;
        baseForce.Normalize();
    }

    private void FixedUpdate()
    {  
        CheckPhase();
        Vector2 steer = steeringBehaviors.Calculate(currentPhase);

        corpse.AddVelocity(steer * Time.deltaTime);
        //if (steeringBehaviors.WallAvoidance() != Vector2.zero) corpse.velocity = steeringBehaviors.WallAvoidance();
        //else corpse.velocity = Vector2.right + Vector2.up;

    }

    void CheckPhase()
    {
        if (currentPhase == SimulationPhase.PursuingInGroup)
        {
            return;
        }
        else if (corpse.neighbors.Count > minGroup)
        {
            currentPhase = SimulationPhase.PursuingInGroup;
            foreach (EnemyBody neighborBody in corpse.neighbors)
            {
                neighborBody.gameObject.GetComponent<ZombieController>().currentPhase = SimulationPhase.PursuingInGroup;
            }
        }
    }
}
