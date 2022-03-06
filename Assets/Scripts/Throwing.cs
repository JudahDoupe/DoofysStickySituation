using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Throwing : MonoBehaviour
{

    private List<GameObject> throwables = new List<GameObject>();
    public GameObject throwLoc;
    public float upThrow = 10;
    public float fwdThrow = 10;
    private GameObject grabbedObj;
    public GameObject lr;
    public GameObject RightHandTarget;
    public GameObject LeftHandTarget;
    [Range(0,1)]
    public float GrabTime;

    private bool holding = false;
    private bool grabbing = false;
    private bool throwing = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (throwables.Count > 0 && !holding)
            {
                grabbedObj = Nearest();
                StartCoroutine(Grab(grabbedObj));
            }
            else if (throwables.Count > 0 && holding && !grabbing)
            {
                holding = false;
                Throw();
                lr.SetActive(false);
            }
        }

        if (holding && !grabbing && !throwing)
        {
            RightHandTarget.transform.position = throwLoc.transform.position;
        }
    }

    IEnumerator Grab(GameObject ball)
    {
        holding = true;
        grabbing = true;

        var time = 0f;
        var halfTime = GrabTime / 2f;

        var startPos = RightHandTarget.transform.position;
        var endPos = ball.transform.position;

        ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        ball.transform.parent = RightHandTarget.transform;
        ball.transform.localPosition = Vector3.zero;

        while (time < halfTime)
        {
            var t = time / halfTime;
            RightHandTarget.transform.position = Vector3.Lerp(startPos, endPos, t);
            ball.transform.localPosition = Vector3.Lerp(ball.transform.localPosition, Vector3.zero, t);

            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }

        ball.GetComponent<Rigidbody>().mass = 0.001f;

        startPos = RightHandTarget.transform.position;
        endPos = throwLoc.transform.position;

        while (time < GrabTime)
        {
            var t = (time - halfTime) / halfTime;
            var offset = transform.right * math.sin(t * math.PI);
            RightHandTarget.transform.position = Vector3.Lerp(startPos, endPos, t) + offset;

            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }

        grabbing = false;
        lr.SetActive(true);
    }

    void Throw()
    {
        grabbedObj.transform.parent.DetachChildren();
        Rigidbody throwRb = grabbedObj.GetComponent<Rigidbody>();
        SphereCollider col = grabbedObj.GetComponent<SphereCollider>();
        throwRb.constraints = RigidbodyConstraints.None;
        grabbedObj.transform.LookAt(forwardObj.transform);
        throwRb.velocity = (transform.up * upThrow + transform.forward * fwdThrow);
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
            for (int i = 0; i < throwables.Count; i++)
                Debug.Log("throwable" + throwables[i] + " " + i);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Throwable"))
        {
            throwables.Remove(col.gameObject);
            for (int i = 0; i < throwables.Count; i++)
                Debug.Log("throwable" + throwables[i] + " " + i);
        }
    }
}
