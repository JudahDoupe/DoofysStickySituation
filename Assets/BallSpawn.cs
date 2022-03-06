using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawn : MonoBehaviour
{
    public Transform prefabBall;
    private Transform lastBall;
    // Start is called before the first frame update
    void Start()
    {
        Transform ball = Instantiate(prefabBall, transform.position, Quaternion.identity);
        ball.gameObject.GetComponent<Rigidbody>().AddTorque(Vector3.left * 5);
        lastBall = ball;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            createBall();
        }
        if (Vector3.Distance(transform.position, lastBall.position) > 15)
        {
            createBall();
        }
    }

    public void createBall()
    {
        Transform ball = Instantiate(prefabBall, transform.position, Quaternion.identity);
        ball.gameObject.GetComponent<Rigidbody>().AddTorque(Vector3.left * 5);
        lastBall = ball;
    }
}
