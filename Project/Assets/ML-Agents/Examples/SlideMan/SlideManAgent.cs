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
    [SerializeField]
    float targetScoreDistance = 3;

    Vector3 startLoc;

    enum AccelerateAction
    {
        DO_NOTHING,
        ACCELERATE,
    }

    enum TurnAction
    {
        DO_NOTHING,
        TURN_CCW,
        TURN_CW,
    }

    public void Start()
    {
        m_AgentRb = GetComponent<Rigidbody>();
        interf = GetComponent<SlideManInterface>();
        startLoc = transform.position;
    }

    public override void OnEpisodeBegin()
    {
        Reset();
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
            Vector3.Angle(m_AgentRb.transform.forward,
            (target.transform.localPosition - m_AgentRb.transform.localPosition)));

        float distanceToTarget = Vector3.Distance(m_AgentRb.transform.localPosition, target.transform.localPosition);
        sensor.AddObservation(distanceToTarget);
    }

    Vector3 lastknownpos;

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var actionAcc = Mathf.Clamp(actionBuffers.ContinuousActions[0], 0, 1f);
        var actionTurn = Mathf.Clamp(actionBuffers.ContinuousActions[1], -1f, 1f);

        interf.Accelerate(actionAcc);
        if (actionTurn < 0) {
            interf.TurnCCW(actionTurn*-1);
                }
        else
        {
            interf.TurnCW(actionTurn);
        }

        //=========================================

        // Rewards
        float distanceToTarget = Vector3.Distance(m_AgentRb.transform.localPosition, target.transform.localPosition);

        //Reached target
        if (distanceToTarget < targetScoreDistance)
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
        //if (MaxStep > 0)
        //{
        //    AddReward(-0.1f / MaxStep);
        //}
        //else
        //{
        //    AddReward(-0.01f);
        //}
        AddReward(-0.0005f);
        
        //mlagents-learn config/slideman_config.yaml --run-id=SlideManNew --force
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "wall")
        {
            AddReward(-1);
            EndEpisode();
        }
    }


    //public override void Heuristic(in ActionBuffers actionsOut)
    //{
    //    var discreteActionsOut = actionsOut.DiscreteActions;
    //    if (Input.GetAxis("Vertical") > 0)
    //    {
    //        discreteActionsOut[0] = (int)AccelerateAction.ACCELERATE;  // Accelerate
    //    }
    //    else
    //    {
    //        discreteActionsOut[0] = (int)AccelerateAction.DO_NOTHING;  // No action
    //    }

    //    float turnDir = Input.GetAxis("Horizontal");
    //    if (turnDir < 0)
    //    {
    //        discreteActionsOut[1] = (int)TurnAction.TURN_CCW; //TurnCCW();
    //    }
    //    else
    //    if (turnDir > 0)
    //    {
    //        discreteActionsOut[1] = (int)TurnAction.TURN_CW;  //TurnCW();
    //    }
    //    else
    //    {
    //        discreteActionsOut[1] = (int)TurnAction.DO_NOTHING;  //do nothing
    //    }

    //}

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;

        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");

    }

}
