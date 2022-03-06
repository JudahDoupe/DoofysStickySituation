using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawn : MonoBehaviour
{
    public Transform prefabBall;

    // Start is called before the first frame update
    void Start()
    {
        Transform ball = Instantiate(prefabBall, transform.position, Quaternion.identity);
        ball.gameObject.GetComponent<Rigidbody>().AddTorque(Vector3.left * 5);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            createBall();
        }
    }

    public void createBall()
    {
        Transform ball = Instantiate(prefabBall, transform.position, Quaternion.identity);
        ball.gameObject.GetComponent<Rigidbody>().AddTorque(Vector3.left * 5);
    }
}
