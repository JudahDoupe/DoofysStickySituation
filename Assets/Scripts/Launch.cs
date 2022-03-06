using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : MonoBehaviour
{

    private Rigidbody rb;
    public float up;
    public float forward;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            startLaunch();
        }
    }

    public void startLaunch()
    {
        rb.velocity = (Vector3.back * forward + Vector3.up * up);
    }
}
