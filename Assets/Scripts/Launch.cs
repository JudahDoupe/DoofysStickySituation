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
    public GameObject doofy;
    private DoofyMovement doofyMove;
    // Start is called before the first frame update
    void Start()
    {
        doofyMove = doofy.GetComponent<DoofyMovement>();
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
        doofyMove.enabled = false;
        launching = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.velocity = (Vector3.back * forward + Vector3.up * up);
        rb.AddTorque(Vector3.forward * 500 + Vector3.left * 500);

    }

    public void endLaunch()
    {
        doofyMove.enabled = true;
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
