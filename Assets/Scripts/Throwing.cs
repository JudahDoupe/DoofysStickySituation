using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Unity.Mathematics;
using UnityEngine;

public class Throwing : MonoBehaviour
{

    public float upThrow = 10;
    public float fwdThrow = 10;
    public GameObject lr;
    public GameObject RightHandTarget;
    public GameObject LeftHandTarget;
    [Range(0,1)]
    public float GrabTime;

    public Vector2 BallOffset;

    private List<GameObject> balls = new List<GameObject>();
    private GameObject ball;

    private bool holding = false;
    private bool grabbing = false;
    private bool throwing = false;

    private Vector3 leftLocal;
    private Vector3 rightLocal;

    void Start()
    {
        leftLocal = LeftHandTarget.transform.localPosition;
        rightLocal = RightHandTarget.transform.localPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (balls.Count > 0 && !holding)
            {
                ball = Nearest();
                StartCoroutine(Grab());
            }
            else if (balls.Count > 0 && holding && !grabbing)
            {
                holding = false;
                Throw();
                lr.SetActive(false);
            }
        }

        if (holding && !grabbing && !throwing)
        {
            var center = GetTargetBallLocation();
            var radius = GetBallRadius();
            RightHandTarget.transform.position = center + transform.right * radius;
            LeftHandTarget.transform.position = center - transform.right  * radius;
            ball.transform.position = GetTargetBallLocation();
            lr.transform.LookAt(lr.transform.position - transform.right);
            lr.transform.position = ball.transform.position;
        }
        if (holding || grabbing  || throwing)
        {
            RightHandTarget.transform.rotation = 
                Quaternion.LookRotation(transform.position - RightHandTarget.transform.position, Vector3.up);
        }
    }

    IEnumerator Grab()
    {
        holding = true;
        grabbing = true;

        var time = 0f;
        var halfTime = GrabTime / 2f;
        var radius = GetBallRadius();

        var startRotation = transform.rotation;
        var leftStartPos = LeftHandTarget.transform.position;
        var rightStartPos = RightHandTarget.transform.position;

        var endRotation =
            Quaternion.LookRotation(
                Vector3.Scale(ball.transform.position - transform.position, new Vector3(1, 0, 1)).normalized,
                Vector3.up);
        var leftEndPos = ball.transform.position - (endRotation * Vector3.right) * radius;
        var rightEndPos = ball.transform.position + (endRotation * Vector3.right) * radius;

        ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        ball.GetComponent<Collider>().enabled = false;

        while (time < halfTime)
        {
            var t = time / halfTime;
            RightHandTarget.transform.position = Vector3.Lerp(rightStartPos, rightEndPos, t);
            LeftHandTarget.transform.position = Vector3.Lerp(leftStartPos, leftEndPos, t);
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);

            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }

        leftStartPos = LeftHandTarget.transform.position;
        rightStartPos = RightHandTarget.transform.position;
        
        var center = GetTargetBallLocation();
        leftEndPos = LeftHandTarget.transform.position = center - transform.right * radius;
        rightEndPos = RightHandTarget.transform.position = center + transform.right * radius;

        while (time < GrabTime)
        {
            var t = (time - halfTime) / halfTime; 
            RightHandTarget.transform.position = Vector3.Lerp(rightStartPos, rightEndPos, t);
            LeftHandTarget.transform.position = Vector3.Lerp(leftStartPos, leftEndPos, t);
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            ball.transform.position = Vector3.Lerp(ball.transform.position, GetTargetBallLocation(), t);

            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }

        grabbing = false;
        lr.SetActive(true);
    }
    private Vector3 GetTargetBallLocation() => transform.position 
                                               + transform.forward * (BallOffset.x + GetBallRadius())
                                               - transform.up * BallOffset.y;
    private float GetBallRadius() => ball.transform.localScale.x / 2f;

    void Throw()
    {
        balls.Remove(ball);
        ball.GetComponent<Collider>().enabled = true;
        Rigidbody throwRb = ball.GetComponent<Rigidbody>();
        SphereCollider col = ball.GetComponent<SphereCollider>();
        throwRb.constraints = RigidbodyConstraints.None;
        throwRb.velocity = (transform.up * upThrow + transform.forward * fwdThrow);

        LeftHandTarget.transform.localPosition = leftLocal;
        RightHandTarget.transform.localPosition = rightLocal;
    }
    GameObject Nearest() =>
        balls.Aggregate((min, x) =>
            Vector3.Distance(transform.position, x.transform.position) <
            Vector3.Distance(transform.position, min.transform.position)
                ? x
                : min);

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Throwable"))
        {
            balls.Add(col.gameObject); 
            col.gameObject.GetComponentInChildren<Renderer>().material.SetFloat("_OutlineThickness", 0.0075f);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Throwable"))
        {
            balls.Remove(col.gameObject);
            col.gameObject.GetComponentInChildren<Renderer>().material.SetFloat("_OutlineThickness", 0);
        }
    }
}
