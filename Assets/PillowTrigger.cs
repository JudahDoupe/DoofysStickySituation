using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillowTrigger : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            RollPillow();
        }
    }

    public void RollPillow()
    {
        GetComponent<Rigidbody>().AddTorque(Vector3.left * 50000);
    }
    private void OnCollisionEnter (Collision col)
    {
        if (col.collider.tag == "Throwable" && col.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 10)
        {
            RollPillow();
        }
    }
}
