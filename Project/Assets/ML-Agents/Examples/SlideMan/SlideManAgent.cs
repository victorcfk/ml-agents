using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Sensors.Reflection;


/*
 * We are going to make a slideman game where the agent tries to eat the food as fast as possible while sliding towards it.
 */
public class SlideManAgent : Agent
{
    [Header("Specific to JumpmanAgent")]
    public GameObject target;

    Rigidbody m_AgentRb;
    EnvironmentParameters m_ResetParams;

    SlideManInterface interf;

    Vector3 startLoc;
    public void Start()
    {
        m_AgentRb = GetComponent<Rigidbody>();
        interf = GetComponent<SlideManInterface>();
        startLoc = transform.position;


    }

    public override void OnEpisodeBegin()
    {
        //reset the agent to the center of the area
        
        //set the food to a random location
        //target.transform.position = new Vector3(Random.Range(0, 10), 0, Random.Range(0, 10));
    }

    private void Reset()
    {
        m_AgentRb.transform.position = startLoc;
        m_AgentRb.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

        m_AgentRb.velocity = new Vector3(0f, 0f, 0f);
        m_AgentRb.angularVelocity = new Vector3(0f, 0f, 0f);

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(target.transform.localPosition);
        sensor.AddObservation(m_AgentRb.transform.localPosition);

        // Agent velocity
        sensor.AddObservation(m_AgentRb.velocity.x);
        sensor.AddObservation(m_AgentRb.velocity.z);

        //Agent Angular velocity
        sensor.AddObservation(m_AgentRb.angularVelocity.y);
    }


    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        //var continuousActions = actionBuffers.ContinuousActions;

        //var actionZ = 2f * Mathf.Clamp(continuousActions[0], -1f, 1f);
        //var actionX = 2f * Mathf.Clamp(continuousActions[1], -1f, 1f);

        // Actions, size = 3
        if (actionBuffers.ContinuousActions[0] > 0) //accelerating
        {
            interf.Accelerate();
        }
        if (actionBuffers.ContinuousActions[1] > 0) //turning CCW
        {
            interf.TurnCCW();
        }
        if (actionBuffers.ContinuousActions[2] > 0) //turning CW
        {
            interf.TurnCW();
        }


        // Rewards
        float distanceToTarget = Vector3.Distance(m_AgentRb.transform.localPosition, target.transform.localPosition);

        //Reached target
        if (distanceToTarget < 2f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        //timing out this
        SetReward(-0.01f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;

        float isAcc = Input.GetAxis("Vertical");
        float turnDir = Input.GetAxis("Horizontal");

        Debug.Log("isAdd: "+ isAcc);

        if (isAcc != 0)
        {
            continuousActionsOut[0] = 1;
        }

        if (turnDir < 0)
        {
            continuousActionsOut[1] = 1; //TurnCCW();
        }
        else
        if (turnDir > 0)
        {
            continuousActionsOut[2] = 1;
        }

    }

    //[Observable(numStackedObservations: 9)]
    //Vector2 Rotation
    //{
    //    get
    //    {
    //        return new Vector2(gameObject.transform.rotation.z, gameObject.transform.rotation.x);
    //    }
    //}

    //[Observable(numStackedObservations: 9)]
    //Vector3 PositionDelta
    //{
    //    get
    //    {
    //        return target.transform.position - gameObject.transform.position;
    //    }
    //}




}
