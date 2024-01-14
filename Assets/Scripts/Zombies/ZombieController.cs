using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    SteeringBehaviors steeringBehaviors;
    EnemyBody corpse;
    public float mass;
    public List<Vector2> lastSteers = new List<Vector2>();
    public List<int> lastSteersWeight = new List<int>() {1, 3, 6};
    public Transform transformPoint;//robocze;

    public Vector2 placeholderDirection;

    public SimulationPhase currentPhase = SimulationPhase.MostlyWandering;

    public int minGroup = 3;

    public float phaseTimer = 0.0f;

    public float nextPhase = 0.0f;

    public SimulationPhase[] possiblePhases = {SimulationPhase.MostlyWandering, SimulationPhase.MostlyHiding};



    void Start ()
    {
        corpse = GetComponent<EnemyBody>();
        steeringBehaviors = GetComponent<SteeringBehaviors>();
        nextPhase = Random.Range(3f, 10f);
    }



    private void FixedUpdate()
    {  
        CheckPhase();
        //Vector2 steer = steeringBehaviors.Calculate(SimulationPhase.MostlyHiding);
        Vector2 steer = steeringBehaviors.Calculate(currentPhase);
        /*
        lastSteers.RemoveAt(0);
        lastSteers.Add(steer);
        Vector2 sumSteer = lastSteers[0] *lastSteersWeight[0] + lastSteers[1] *lastSteersWeight[1] + lastSteers[2] *lastSteersWeight[2];
        */
        corpse.AddVelocity(steer * Time.deltaTime);
        //if (steeringBehaviors.WallAvoidance() != Vector2.zero) corpse.velocity = steeringBehaviors.WallAvoidance();
        //else corpse.velocity = Vector2.right + Vector2.up;

    }

    void CheckPhase()
    {
        if (currentPhase == SimulationPhase.PursuingInGroup)
        {
            foreach (EnemyBody neighborBody in corpse.neighbors)
            {
                neighborBody.gameObject.GetComponent<ZombieController>().currentPhase = SimulationPhase.PursuingInGroup;
            }
            
        }
        else if (corpse.neighbors.Count > minGroup)
        {
            currentPhase = SimulationPhase.PursuingInGroup;
            foreach (EnemyBody neighborBody in corpse.neighbors)
            {
                neighborBody.gameObject.GetComponent<ZombieController>().currentPhase = SimulationPhase.PursuingInGroup;
            }
        }
        else 
        {
            phaseTimer += Time.deltaTime;
            if (phaseTimer>=nextPhase)
            {
                phaseTimer = 0.0f;
                nextPhase = Random.Range(3f, 10f);
                currentPhase  = possiblePhases[Random.Range(0,possiblePhases.Length)];
            }
        }
    }
    
}
