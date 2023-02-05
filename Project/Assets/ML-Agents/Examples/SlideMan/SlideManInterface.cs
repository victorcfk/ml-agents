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
        hasCollidedWithWall = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!allowcontrol) return;

        if (Input.GetAxis("Vertical") > 0)
        {
            Accelerate(Input.GetAxis("Vertical"));
        }

        float turnDir = Input.GetAxis("Horizontal");
        if (turnDir < 0)
        {
            TurnCCW(turnDir);
        }
        else
        if (turnDir > 0)
        {
            TurnCW(turnDir);
        }
    }

    public bool hasCollidedWithWall = false;
    //private void OnCollisionEnter(Collision other)
    //{
    //    //if (other.tag == "goal")
    //    //{
    //    //    Score++;
    //    //    Destroy(other.gameObject);
    //    //}

    //    if (other.gameObject.tag == "wall")
    //    {
    //        hasCollidedWithWall = true;
    //    }
    //}

    public void Accelerate(float val)
    {
        rb.AddForce(transform.forward * moveForce * val, ForceMode.Force);
    }

    public void TurnCCW(float val)
    {
        rb.AddTorque(transform.up * -1 * turnForce * val, ForceMode.Force);
    }

    public void TurnCW(float val)
    {
        rb.AddTorque(transform.up * turnForce * val, ForceMode.Force);
    }

    
}
