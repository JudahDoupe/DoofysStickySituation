using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camSwitch : MonoBehaviour
{

    public GameObject cam1;
    public GameObject cam2;
    private float end = 19f;
    private float time;
    private bool dot;
    // Start is called before the first frame update
    void Start()
    {
        dot = true;
        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if ((time > end) && dot)
        {
            dot = false;
            cam2.SetActive(true);
            cam1.SetActive(false);
        }
    }
}
