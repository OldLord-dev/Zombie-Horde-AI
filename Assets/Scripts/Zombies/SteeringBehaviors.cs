using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class SteeringBehaviors : MonoBehaviour
{
    GameObject player;
    ZombieController zombieController;
    private Vector2 m_vWanderTarget;
    [SerializeField]
    float m_dWanderRadius=30;
    [SerializeField]
    float m_dWanderDistance=10;
    [SerializeField]
    float m_dWanderJitter=0.0001f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        zombieController = player.GetComponent<ZombieController>(); 
    }
    public Vector2 Wander() {
       // m_vWanderTarget = new Vector2(player.transform.position.x, player.transform.position.y);
        //first, add a small random vector to the target’s position (RandomClamped
        //returns a value between -1 and 1)

        //psu³o kod
        m_vWanderTarget += new Vector2(Mathf.Clamp(Random.Range(-1f, 2f) * m_dWanderJitter, -1f, 1f), Mathf.Clamp(Random.Range(-1f, 2f) * m_dWanderJitter, -1f, 1f));
        m_vWanderTarget.Normalize();
        m_vWanderTarget *= m_dWanderRadius;

        //move the target into a position WanderDist in front of the agent
        Vector2 targetLocal = m_vWanderTarget + new Vector2(m_dWanderDistance, 0);
        //project the target into world space
        //Vector2 targetWorld = TransformPoint(targetLocal,
        //m_pVehicle->Heading(),
        //m_pVehicle->Side(),
        //m_pVehicle->Pos());
        //and steer toward it
        Vector3 targetWorld = transform.TransformVector(targetLocal);
        return targetWorld - transform.position;
    }

    /*     public Vector2 Seek(){}
        public Vector2 Flee(){}
        public Vector2 Arrive(){}
        public Vector2 Pursuit(){}
        public Vector2 Evade(){}

        public Vector2 ObstacleAvoidance(){}
        public Vector2 WallAvoidance(){}
        public Vector2 Interpose(){}
        public Vector2 Hide(){}
        public Vector2 PathFollowing(){}
        public Vector2 OffsetPursuit(){}
        public Vector2 Separation(){}
        public Vector2 Alignment(){}
        public Vector2 Cohesion(){}
        public Vector2 Flocking(){} */
    public Vector2 Seek(Vector2 TargetPos)
    {
        Vector2 DesiredVelocity = TargetPos - new Vector2(transform.position.x, transform.position.y)*3;
        DesiredVelocity.Normalize();
        //*m_pVehicle->MaxSpeed();
        return (DesiredVelocity - new Vector2(zombieController.transform.position.x, zombieController.transform.position.y));
    }

}
