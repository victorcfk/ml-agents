using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideManInterface : MonoBehaviour
{
    Rigidbody rb;

    public float moveForce = 5;
    public float turnForce = 5;
    public int Score = 0;

    [SerializeField]
    bool allowcontrol;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!allowcontrol) return;

        float isAcc = Input.GetAxis("Vertical");
        float turnDir = Input.GetAxis("Horizontal");
        
        if (isAcc != 0)
        {
            Accelerate();
        }

        if (turnDir < 0)
        {
            TurnCCW();
        }
        else
        if (turnDir > 0)
        {
            TurnCW();
        }

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.tag == "goal")
    //    {
    //        Score++;
    //        Destroy(other.gameObject);
    //    }    
    //}

    public void Accelerate()
    {
        rb.AddForce(transform.forward *-1 * moveForce, ForceMode.Force);
    }

    public void TurnCW()
    {
        rb.AddTorque(transform.up * turnForce, ForceMode.Force);
    }

    public void TurnCCW()
    {
        rb.AddTorque(transform.up* -1 * turnForce, ForceMode.Force);
    }
}
