using UnityEngine;

using static UnityEngine.Rendering.DebugUI;

using System.Collections.Generic;
public enum SimulationPhase{MostlyHiding, MostlyWandering, PursuingInGroup};//FollowingPursuing, LeadingPursuing, Gathering};
public enum Deceleration{Slow=3, Normal=2, Fast=1};
public class SteeringBehaviors : MonoBehaviour
{
    GameObject player;
    ZombieController zombieController;
    CollisionsLoop collisionsLoop;
    public FeelerBody[] wallFeelers;
    public DetectionBody obstacleDetector;
    public Vector2 steeringForceTotal;

    public float brakingWeight = 0.2f;

    private Vector2 wanderTarget;
    [SerializeField]
    float wanderRadius=16f; //This is the radius of the constraining circle
    [SerializeField]
    float wanderDistance=4f; //This is the distance the wander circle is projected in front of the agent.
    [SerializeField]
    float wanderJitter=2f; // the maximum amount of random displacement that can be added to the target each second.
    Vector2 circlePos;

    Dictionary<SimulationPhase, float[]> behaviourDict = new Dictionary<SimulationPhase, float[]>();

    //public List<Vector2> lastAvoid = new List<Vector2>{Vector2.zero, Vector2.zero, Vector2.zero};

    void OnDrawGizmos ()
    {
        //Gizmos.DrawSphere(circlePos, wanderRadius);
        //Gizmos.DrawSphere(transform.position, collisionsLoop.neighborDistance);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        zombieController = player.GetComponent<ZombieController>(); 
        collisionsLoop = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CollisionsLoop>();
        wallFeelers = transform.GetChild(1).GetComponentsInChildren<FeelerBody>(); //child with index 1 must be WallFeelers
        obstacleDetector = transform.GetChild(2).GetComponent<DetectionBody>();
        wanderTarget = Vector2.zero;
        wanderRadius=1f; //This is the radius of the constraining circle
        wanderDistance=3f; //This is the distance the wander circle is projected in front of the agent.
        wanderJitter=0.01f;

        //Wander| Hide| Flocking |Pursuit
        behaviourDict.Add(SimulationPhase.MostlyHiding, new float[4]{0.2f, 0.7f, 0.1f , 0.0f});
        behaviourDict.Add(SimulationPhase.MostlyWandering, new float[4]{0.7f, 0.2f, 0.1f, 0.0f});
        behaviourDict.Add(SimulationPhase.PursuingInGroup, new float[4]{0.2f, 0.0f, 0.5f, 1.0f});
    }
    public bool AccumulateForce(Vector2 force)
    {
        EnemyBody body = GetComponent<EnemyBody>();
        float magnitudeSoFar = steeringForceTotal.magnitude;
        float magnitudeRemaining = body.maxForce - magnitudeSoFar;
        if(magnitudeRemaining <= 0f) {return false;}
        float magnitudeToAdd =  force.magnitude;
        if (magnitudeToAdd < magnitudeRemaining)
        {
            steeringForceTotal += force;
        }
        else
        {

            steeringForceTotal += force.normalized * magnitudeRemaining;
        }
        return true;
}
    public Vector2 Calculate(SimulationPhase phase)
    {
        Vector2 steeringForceTotal = Vector2.zero;
        Vector2 currentPos = transform.position;
        Vector2 avoidWall = WallAvoidance()*10f;
        if (avoidWall.magnitude>0.05f)
        {
            Debug.DrawLine(avoidWall + currentPos, currentPos,Color.green);
            return avoidWall;
        }

        Vector2 avoidObstacle = ObstacleAvoidance()*2f;
        Debug.DrawLine(avoidObstacle + currentPos, currentPos,Color.green);
        /*
        else if(lastAvoid[1]!=Vector2.zero)
        {
            return lastAvoid[1];
        }
        else if(lastAvoid[0]!=Vector2.zero)
        {
            return lastAvoid[0];
        }
        */
        float[] currentMode = behaviourDict[phase];
        Vector2 tempForce = Wander()*currentMode[0];
        Debug.DrawLine(tempForce + currentPos, currentPos,Color.white);
        steeringForceTotal += tempForce;
        tempForce = Hide()*currentMode[1];
        Debug.DrawLine(tempForce + currentPos, currentPos,Color.red);
        steeringForceTotal += tempForce;
        tempForce = Flocking()*currentMode[2];
        Debug.DrawLine(tempForce + currentPos, currentPos,Color.blue);
        steeringForceTotal += Flocking()*currentMode[2];
        tempForce = Pursuit()*currentMode[3];
        Debug.DrawLine(tempForce + currentPos, currentPos,Color.yellow);
        steeringForceTotal += Pursuit()*currentMode[3];
        steeringForceTotal+= avoidObstacle;
        
        return steeringForceTotal;
    }
/*
    public Vector2 Calculate(SimulationPhase phase)
    {
        steeringForceTotal = Vector2.zero;
        Vector2 currentPos = transform.position;
        float wWallAvoidance = 0f;
        float wObstacleAvoidance = 0f;
        float wHide = 0f;
        float wPursuit = 0f;
        float wSeparation = 0f;
        float wAlignment = 0f;
        float wCohesion = 0f;
        float wWander = 0f;
        if (phase == SimulationPhase.Hiding)
        {
            wWallAvoidance = 1f;
            wObstacleAvoidance = 1f;
            wHide = 0.4f;
            wSeparation = 0.2f;
            wAlignment = 0.2f;
            wCohesion = 0.2f;
            wWander = 0.7f;
        }
        else if (phase == SimulationPhase.PursuingInGroup)
        {
            wWallAvoidance = 1f;
            wObstacleAvoidance = 1f;
            wPursuit = 0.4f;
            wCohesion = 0.2f;
            wWander = 0.2f;
        }
        else if (phase == SimulationPhase.Gathering)
        {
            wWallAvoidance = 1f;
            wObstacleAvoidance = 1f;
            wSeparation = 0.6f;
            wAlignment = 0.6f;
            wCohesion = 0.6f;
            wWander = 0.7f;
        }
        //Wall Avoidance
        //Obstacle
        //Hide
        //Pursuit
        //alignment
        //cohesion
        //separation
        //OffsetPursuit
        //wander

        Vector2 tempForce = WallAvoidance()*wWallAvoidance;
        Debug.DrawLine(tempForce + currentPos, currentPos,Color.green);
        if (!AccumulateForce(tempForce)) {Debug.Log("WA"); return steeringForceTotal;}
        tempForce = ObstacleAvoidance()*wObstacleAvoidance;
        Debug.DrawLine(tempForce + currentPos, currentPos,Color.green);
        if (!AccumulateForce(tempForce)) {Debug.Log("OA");return steeringForceTotal;}
        tempForce = Hide()*wHide;
        Debug.DrawLine(tempForce + currentPos, currentPos,Color.blue);
        if (!AccumulateForce(tempForce)) {Debug.Log("H");return steeringForceTotal;}
        tempForce = Pursuit()*wPursuit;
        Debug.DrawLine(tempForce + currentPos, currentPos,Color.blue);
        if (!AccumulateForce(tempForce)) {Debug.Log("P");return steeringForceTotal;}
        tempForce = Separation()*wSeparation;
        Debug.DrawLine(tempForce + currentPos, currentPos,Color.red);
        if (!AccumulateForce(tempForce)) {Debug.Log("S");return steeringForceTotal;}
        tempForce = Alignment()*wAlignment;
        Debug.DrawLine(tempForce + currentPos, currentPos,Color.red);
        if (!AccumulateForce(tempForce)) {Debug.Log("A");return steeringForceTotal;}
        tempForce = Cohesion()*wCohesion;
        Debug.DrawLine(tempForce + currentPos, currentPos,Color.red);
        if (!AccumulateForce(tempForce)) {Debug.Log("C");return steeringForceTotal;}
        tempForce = Wander()*wWander;
        Debug.DrawLine(tempForce + currentPos, currentPos,Color.white);
        if (!AccumulateForce(tempForce)) {Debug.Log("W");return steeringForceTotal;}
        return steeringForceTotal;
    }
*/
    public Vector2 Wander() {
        EnemyBody body = GetComponent<EnemyBody>();
        Vector2 currentPos = transform.position;
        //Debug.Log(-transform.up);
        //project circle
        Vector2 direction = -transform.up.normalized ;
        
        circlePos = currentPos + direction * wanderDistance;
        //Debug.DrawLine(currentPos, circlePos, Color.red);
        wanderTarget += new Vector2(Random.Range(-1f, 1f) * wanderJitter, Random.Range(-1f, 1f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector2 wanderTargetPos = wanderTarget + circlePos;
       //Debug.DrawLine((wanderTargetPos - currentPos).normalized+ currentPos, currentPos,Color.red);
        return (wanderTargetPos - currentPos).normalized*3.0f;
    }




        
        
        

        


    public Vector2 Cohesion()
    {
        Vector2 steeringForce = Vector2.zero;
        Vector2 centreMass = Vector2.zero;
        EnemyBody thisBody = GetComponent<EnemyBody>();

        int neighborCount = 0;

        foreach (EnemyBody neighborBody in thisBody.neighbors)
        {
            Vector2 nPos = neighborBody.transform.position;
            centreMass += nPos;
            neighborCount ++;
        }
        if (neighborCount > 0)
        {
            centreMass = centreMass/neighborCount;
            steeringForce = Seek(centreMass);
        }
        return steeringForce;
    }

    public Vector2 Separation()
    {
        Vector2 steeringForce = Vector2.zero;
        EnemyBody thisBody = GetComponent<EnemyBody>();
        foreach (EnemyBody neighborBody in thisBody.neighbors)
        {
            Vector2 toThis = transform.position - neighborBody.transform.position;
            steeringForce += toThis.normalized/toThis.magnitude;
        }

        return steeringForce;
    }
    public Vector2 Alignment()
    {
        Vector2 averageHeading = Vector2.zero;
        EnemyBody thisBody = GetComponent<EnemyBody>();

        int neighborCount = 0;
        foreach (EnemyBody neighborBody in thisBody.neighbors)
        {
            averageHeading += neighborBody.velocity.normalized;
            neighborCount ++;
        }
        if (neighborCount > 0)
        {
            averageHeading = averageHeading/neighborCount;
            averageHeading -= thisBody.velocity.normalized;
        }
        return averageHeading;
    }
    public Vector2 Flocking()
    {
        float[] flockingWeights= new float[3]{0.5f , 1f, 0.5f};
        Vector2 flockingSteering = Separation() * flockingWeights[0] + Cohesion() * flockingWeights[1] + Alignment()* flockingWeights[2];
        return flockingSteering;
    }
    
    public Vector2 OffsetPursuit(GameObject leader, Vector2 localOffsetPos)
    {
        Vector2 worldOffsetPos = leader.transform.rotation * localOffsetPos + leader.transform.position;
        
        Vector2 currentPos = transform.position;
        EnemyBody cBody = GetComponent<EnemyBody>();
        EnemyBody lBody = leader.GetComponent<EnemyBody>();

        Vector2 toOffset = worldOffsetPos - currentPos;
        //the look-ahead time is proportional to the distance between the leader
        //and the pursuer; and is inversely proportional to the sum of both
        //agents’ velocities
        float lookAheadTime = toOffset.magnitude / (cBody.maxSpeed + lBody.velocity.magnitude);
        return Arrive(worldOffsetPos + lBody.velocity * lookAheadTime, Deceleration.Fast);
    }
    public Vector2 Hide()
    {
        float bestDistance = Mathf.Infinity;
        Vector2 bestHidingSpot = Vector2.positiveInfinity;
        Vector2 playerPos = player.transform.position;
        Vector2 currentPos = transform.position;
        foreach (StaticObstacle obstacle in collisionsLoop.allObstacles)
        {
            (bool isSpot, Vector2 hidingSpot) = obstacle.GetHidingPosition(playerPos);
            if (isSpot)
            {
                float distance = (hidingSpot-currentPos).magnitude;
                if(distance < bestDistance)
                {
                    bestDistance =distance; 
                    bestHidingSpot = hidingSpot;
                }

            }
        
        }

        if (bestDistance == Mathf.Infinity)
        {
            return Evade();
        }
        Debug.DrawLine(transform.position, bestHidingSpot, Color.magenta);
        return Arrive(bestHidingSpot, Deceleration.Fast);
    }

    public Vector2 Pursuit()
    {
        Vector2 currentPos = transform.position;
        Vector2 playerPos = player.transform.position;
        
        EnemyBody cBody = GetComponent<EnemyBody>();
        PlayerBody pBody = player.GetComponent<PlayerBody>();

        Vector2 currentV = cBody.velocity;
        Vector2 playerV = pBody.velocity;

        Vector2 toEvader = playerPos - currentPos;



        float relativeHeading = Vector2.Dot(currentV, playerV);
        if ((Vector2.Dot(toEvader, currentV) > 0) && (relativeHeading < -0.95)) //acos(0.95)=18 degs
        {
            return Seek(playerPos);
        }
        //Not considered ahead so we predict where the evader will be.
        //the look-ahead time is proportional to the distance between the evader
        //and the pursuer; and is inversely proportional to the sum of the
        //agents' velocities
        float lookAheadTime = toEvader.magnitude / (cBody.maxSpeed + playerV.magnitude);
        //now seek to the predicted future position of the evader
        return Seek(playerPos + playerV * lookAheadTime);
    }
    public Vector2 Evade()
    {
        Vector2 currentPos = transform.position;
        Vector2 playerPos = player.transform.position;
        
        EnemyBody cBody = GetComponent<EnemyBody>();
        PlayerBody pBody = player.GetComponent<PlayerBody>();

        Vector2 playerV = pBody.velocity;

        Vector2 toPursuer = playerPos - currentPos;

        float lookAheadTime = toPursuer.magnitude / (cBody.maxSpeed + playerV.magnitude);

        return Flee(playerPos + playerV * lookAheadTime);
    }
    public Vector2 Arrive(Vector2 targetPos, Deceleration decelaration)
    {
        EnemyBody body = GetComponent<EnemyBody>();
        Vector2 currentPos = transform.position;
        Vector2 toTarget = targetPos - currentPos;



        float dist = toTarget.magnitude;
        if (dist > 0)
        {

            float decelerationTweaker = 0.3f;

            float speed = dist / ((int)decelaration * decelerationTweaker);
            speed = Mathf.Min(speed, body.maxSpeed);

            Vector2 desiredVelocity = toTarget * speed / dist;
            return desiredVelocity - body.velocity;
        }
        return Vector2.zero;
    }

    public Vector2 Flee(Vector2 targetPos)
    {
        float panicDistance = 50.0f;
        Vector2 currentPos = transform.position;
        if ((targetPos - currentPos).magnitude > panicDistance)
        {
            return Vector2.zero;
        }
        EnemyBody body = GetComponent<EnemyBody>();

        Vector2 desiredVelocity = (currentPos - targetPos).normalized * body.maxSpeed;

        return desiredVelocity - body.velocity;
    }

    public Vector2 Seek(Vector2 targetPos)
    {
        EnemyBody body = GetComponent<EnemyBody>();
        Vector2 currentPos = transform.position;
        Vector2 desiredVelocity = (targetPos - currentPos).normalized * body.maxSpeed;

        return desiredVelocity - body.velocity;
    }

    public Vector2 WallAvoidance()
        {
            Vector2 steeringForce = Vector2.zero;
            foreach (FeelerBody feeler in wallFeelers)
            {
                if (feeler.detected == true)
                {
                    
                    Vector2 feelerPos = feeler.transform.position;
                    Vector2 penetration = feeler.closestPoint - feelerPos;

                    steeringForce += penetration;
                    Vector3 end = penetration;
                    //Debug.DrawLine(transform.position, transform.position + end, Color.white);
                    //Reset feeler
                    feeler.NoCollision();
                } 
                
            }
            Vector3 en = steeringForce;
            //Debug.DrawLine(transform.position, transform.position + en, Color.green);
            return steeringForce;
        }

        public Vector2 ObstacleAvoidance()
        {
            Vector2 steeringForce = Vector2.zero;
            if (obstacleDetector.detected == true && obstacleDetector.c_collider is RectangleCollider rectCollider)
            {
                float feelerLength = rectCollider.c_size.y;
                float multiplier = 1.0f + (feelerLength - obstacleDetector.distance)/feelerLength;
                Vector2 currentPos = transform.position;
                Vector2 brakingDirection = (currentPos - obstacleDetector.closestIntersection).normalized;

                Vector2 brakingForce =  brakingDirection * (obstacleDetector.distanceCentre - obstacleDetector.obstacleRadius) * brakingWeight;//(obstacle radius - odległość) *BrakingWeight;
                Vector2 lateralForce = obstacleDetector.lateralForce * multiplier;
                Vector3 end = brakingForce;
                //Debug.DrawLine(transform.position, transform.position + end, Color.blue);

                end = lateralForce;
                //Debug.DrawLine(transform.position, transform.position + end, Color.yellow);
                //Reset feeler
                obstacleDetector.NoCollision();
                steeringForce = lateralForce+brakingForce;
            } 

            Vector3 en = steeringForce;
            //Debug.DrawLine(transform.position, transform.position + en, Color.green);
            return steeringForce;
        }
}
