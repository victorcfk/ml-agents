using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors.Reflection;


/*
 * We are going to make a runman game where the agent tries to eat the food as fast as possible while sliding towards it.
 */
public class SlideManAgent : Agent
{
    [Header("Specific to JumpmanAgent")]
    public GameObject food;

    //Rigidbody m_AgentRb;
    EnvironmentParameters m_ResetParams;

    public override void Initialize()
    {
        //m_AgentRb = ball.GetComponent<Rigidbody>();
        m_ResetParams = Academy.Instance.EnvironmentParameters;
        SetResetParameters();
    }
    public void SetAgent()
    {
        //Set the attributes of the ball by fetching the information from the academy
        //m_AgentRb.mass = m_ResetParams.GetWithDefault("mass", 1.0f);
        var position = m_ResetParams.GetWithDefault("position", 0.0f);
        food.transform.position = new Vector3(position, position, position);
    }
    public void SetResetParameters()
    {
        SetAgent();
    }

    [Observable(numStackedObservations: 9)]
    Vector2 Rotation
    {
        get
        {
            return new Vector2(gameObject.transform.rotation.z, gameObject.transform.rotation.x);
        }
    }

    [Observable(numStackedObservations: 9)]
    Vector3 PositionDelta
    {
        get
        {
            return food.transform.position - gameObject.transform.position;
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var continuousActions = actionBuffers.ContinuousActions;

        var actionZ = 2f * Mathf.Clamp(continuousActions[0], -1f, 1f);
        var actionX = 2f * Mathf.Clamp(continuousActions[1], -1f, 1f);

        if ((gameObject.transform.rotation.z < 0.25f && actionZ > 0f) ||
            (gameObject.transform.rotation.z > -0.25f && actionZ < 0f))
        {
            gameObject.transform.Rotate(new Vector3(0, 0, 1), actionZ);
        }

        if ((gameObject.transform.rotation.x < 0.25f && actionX > 0f) ||
            (gameObject.transform.rotation.x > -0.25f && actionX < 0f))
        {
            gameObject.transform.Rotate(new Vector3(1, 0, 0), actionX);
        }
        if ((food.transform.position.y - gameObject.transform.position.y) < -2f ||
            Mathf.Abs(food.transform.position.x - gameObject.transform.position.x) > 3f ||
            Mathf.Abs(food.transform.position.z - gameObject.transform.position.z) > 3f)
        {
            SetReward(-1f);
            EndEpisode();
        }
        else
        {
            SetReward(0.1f);
        }
    }

    public override void OnEpisodeBegin()
    {
        gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        gameObject.transform.position = Vector3.zero;

        //gameObject.transform.Rotate(new Vector3(1, 0, 0), Random.Range(-10f, 10f));
        //gameObject.transform.Rotate(new Vector3(0, 0, 1), Random.Range(-10f, 10f));

        food.transform.position = new Vector3(Random.Range(0, 10), 0, Random.Range(0, 10));

        //m_AgentRb.velocity = new Vector3(0f, 0f, 0f);

        //ball.transform.position = new Vector3(Random.Range(-1.5f, 1.5f), 4f, Random.Range(-1.5f, 1.5f))
        //    + gameObject.transform.position;
    }


}
