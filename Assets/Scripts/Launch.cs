using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : MonoBehaviour
{

    private Rigidbody rb;
    public float up;
    public float forward;
    private DoofyMovement mover;
    private bool launching;

    // Start is called before the first frame update
    void Start()
    {
        launching = false;
        rb = GetComponent<Rigidbody>();
        mover = GetComponent<DoofyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            startLaunch();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            endLaunch();
        }
        if (rb.velocity.magnitude == 0 && launching)
        {
            endLaunch();
        }
    }

    public void startLaunch()
    {
        launching = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.velocity = (Vector3.back * forward + Vector3.up * up);
        rb.AddTorque(Vector3.forward * 500 + Vector3.left * 500);
    }

    public void endLaunch()
    {
        transform.rotation = Quaternion.identity;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        launching = false;
    }

    void OnTriggerEnter (Collider col)
    {
        if (col.CompareTag("Finish"))
        {
            endLaunch();
        }
    }
}
