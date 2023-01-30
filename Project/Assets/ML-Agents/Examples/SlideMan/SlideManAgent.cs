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

    [SerializeField]
    SlideManManager smm;
    [SerializeField]
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
        Reset();
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
        lastknownpos = startLoc;

        interf.hasCollidedWithWall = false;

        smm.MoveFood();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(target.transform.localPosition); // 3 values
        sensor.AddObservation(m_AgentRb.transform.localPosition); // 3 values

        // Agent velocity
        sensor.AddObservation(m_AgentRb.velocity.x);
        sensor.AddObservation(m_AgentRb.velocity.z);

        //Agent Angular velocity
        sensor.AddObservation(m_AgentRb.transform.localRotation.y); //this is very important which I missed
        sensor.AddObservation(m_AgentRb.angularVelocity.y);

        //Direction to
        sensor.AddObservation(
            Vector3.Angle(m_AgentRb.transform.forward*-1,
            (target.transform.localPosition - m_AgentRb.transform.localPosition)));
    }

    Vector3 lastknownpos;

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        //var continuousActions = actionBuffers.ContinuousActions;

        //var actionZ = 2f * Mathf.Clamp(continuousActions[0], -1f, 1f);
        //var actionX = 2f * Mathf.Clamp(continuousActions[1], -1f, 1f);

        // Actions, size = 3
        if (actionBuffers.ContinuousActions[0] > 0.1f) //accelerating
        {
            interf.Accelerate();
        }
        if (actionBuffers.ContinuousActions[1] > 0.1f) //turning CCW
        {
            interf.TurnCCW();
        }
        if (actionBuffers.ContinuousActions[2] > 0.1f) //turning CW
        {
            interf.TurnCW();
        }

        //=========================================

        // Rewards
        float distanceToTarget = Vector3.Distance(m_AgentRb.transform.localPosition, target.transform.localPosition);

        //Reached target
        if (distanceToTarget < 2f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        if (interf.hasCollidedWithWall)
        {
            EndEpisode();
        }

        //if (GetCumulativeReward() < 0.9f)
        //{
        //    //Close to target
        //    if (distanceToTarget < 10f)
        //    {
        //        AddReward(0.05f);
        //    }

        //    if (Vector3.Angle(m_AgentRb.transform.forward * 1, (target.transform.localPosition - m_AgentRb.transform.localPosition)) < 10f)
        //    {
        //        AddReward(0.05f);
        //    }
        //}
        //===================================
        //var distRatio = Mathf.Lerp(2f, 100f, distanceToTarget);

        ////Close to target, is better
        //AddReward(Mathf.Lerp(0.01f, 0.02f, distRatio));

        ////penalize it for getting stuck
        //if (Vector3.SqrMagnitude(transform.localPosition - lastknownpos) < 2)
        //{
        //    AddReward(-0.03f);

        //    if (GetCumulativeReward() < -10.0f)
        //    {
        //        EndEpisode();
        //    }
        //}
        //lastknownpos = transform.localPosition;

        //===================================
        ////timing out this
        if (MaxStep > 0)
        {
            AddReward(-0.1f / MaxStep);
        }
        else
        {
            AddReward(-0.01f);
        }

        //if(StepCount > 30000)
        //{
        //    SetReward(-1f);
        //    EndEpisode();
        //}


        //mlagents-learn config/slideman_config.yaml --run-id=SlideMan --force
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "wall")
        {
            AddReward(-1);
            EndEpisode();
        }
    }



    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;

        float isAcc = Input.GetAxis("Vertical");
        float turnDir = Input.GetAxis("Horizontal");

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
