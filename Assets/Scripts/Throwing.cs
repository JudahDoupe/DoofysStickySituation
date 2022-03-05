using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{

    private List<GameObject> throwables = new List<GameObject>();
    private GameObject throwLoc;
    private bool holding;
    public float upThrow = 100;
    public float fwdThrow = 100;

    // Start is called before the first frame update
    void Start()
    {
        throwLoc = transform.GetChild(0).gameObject;
        holding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (throwables.Count > 0 && !holding)
            {
                holding = true;
                Grab();
            }
            else if (throwables.Count > 0 && holding)
            {
                holding = false;
                Throw();
            }
        }
    }

    void Grab()
    {
        GameObject objToThrow = Nearest();
        objToThrow.transform.parent = throwLoc.transform;
        objToThrow.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        objToThrow.transform.localPosition = Vector3.zero;

    }

    void Throw()
    {
        GameObject throwee = throwLoc.transform.GetChild(0).gameObject;
        Rigidbody throwRb = throwee.GetComponent<Rigidbody>();
        throwRb.constraints = RigidbodyConstraints.None;
        throwRb.velocity = (new Vector3(0, upThrow, fwdThrow));

    }
    GameObject Nearest()
    {
        //temp
        return throwables[0];
    }
    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.gameObject.name);
        if (col.CompareTag("Throwable"))
        {
            throwables.Add(col.gameObject);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Throwable"))
        {
            throwables.Remove(col.gameObject);
        }
    }
}
