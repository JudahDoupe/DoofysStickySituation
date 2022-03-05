using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{

    private List<GameObject> throwables = new List<GameObject>();
    public GameObject throwLoc;
    private bool holding;
    public float upThrow = 100;
    public float fwdThrow = 100;
    private GameObject grabbedObj;
    public GameObject lr;

    // Start is called before the first frame update
    void Start()
    {
        //throwLoc = transform.GetChild(0).gameObject;
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
                lr.SetActive(true);
            }
            else if (throwables.Count > 0 && holding)
            {
                holding = false;
                Throw();
                lr.SetActive(false);
            }
        }
    }

    void Grab()
    {
        grabbedObj = Nearest();
        grabbedObj.transform.parent = throwLoc.transform;
        grabbedObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        grabbedObj.transform.localPosition = Vector3.zero;
        grabbedObj.GetComponent<Rigidbody>().mass = 0.001f;
    }

    void Throw()
    {
        Rigidbody throwRb = grabbedObj.GetComponent<Rigidbody>();
        SphereCollider col = grabbedObj.GetComponent<SphereCollider>();
        throwRb.constraints = RigidbodyConstraints.None;
        throwRb.velocity = (new Vector3(0, upThrow, fwdThrow));
        throwRb.mass = 1.0f;
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
