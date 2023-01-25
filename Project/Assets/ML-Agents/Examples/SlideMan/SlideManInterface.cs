using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideManInterface : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField]
    float moveForce = 5;
    [SerializeField]
    float turnForce = 5;

    public int Score = 0;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float turnDir = Input.GetAxis("Horizontal");
        float isAcc = Input.GetAxis("Vertical");

        if(turnDir < 0)
        {
            TurnACW();
        }
        else
        if (turnDir > 0)
        {
            TurnCW();
        }

        if(isAcc != 0)
        {
            Accelerate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "goal")
        {
            Score++;
            Destroy(other.gameObject);
        }    
    }

    public void Accelerate()
    {
        rb.AddForce(transform.forward *-1 * moveForce, ForceMode.Force);
    }

    public void TurnCW()
    {
        rb.AddTorque(transform.up * turnForce, ForceMode.Force);
    }

    public void TurnACW()
    {
        rb.AddTorque(transform.up* -1 * turnForce, ForceMode.Force);
    }
}
