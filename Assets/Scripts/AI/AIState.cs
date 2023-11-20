using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class AIState
{
    public enum STATE
    {
        WANDER,SEEK, FLEE, ARRIVE
    };

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    public STATE name;
    protected EVENT stage;
    protected GameObject npc;
    protected Transform player;
    protected AIState nextState;
    protected NavMeshAgent agent;
    Vector2 m_vWanderTarget;
    float visDist = 15.0f;
    float visAngle = 180.0f;
    float m_dWanderRadius;
    float m_dWanderDistance;
    float m_dWanderJitter;
    public AIState(GameObject _npc, Transform _player)
    {
        npc = _npc;
        stage = EVENT.ENTER;
        player = _player;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    public AIState Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }

    public bool CanSeePlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);

        if (direction.magnitude < visDist && angle < visAngle)
        {
            return true;
        }
        return false;
    }
    public Vector2 Wander()
    {


        //first, add a small random vector to the target’s position (RandomClamped
        //returns a value between -1 and 1)
        m_vWanderTarget += new Vector2(Random.Range(-1,2) * m_dWanderJitter, Random.Range(-1, 2) * m_dWanderJitter);
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
        Vector3 targetWorld = npc.transform.TransformVector(targetLocal);
        return targetWorld - npc.transform.position;

    }
}


public class Wander : AIState
{
    public Wander(GameObject _npc, Transform _player)
                : base(_npc, _player)
    {
        name = STATE.WANDER;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
   

    }

    public override void Exit()
    {
        //IDLE
        base.Exit();
    }
}




