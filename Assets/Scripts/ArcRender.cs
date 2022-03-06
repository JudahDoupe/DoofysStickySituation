using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcRender : MonoBehaviour
{

    LineRenderer lr;
    public float vel;
    public float angle;
    public int resolution;
    float g;
    float radianAngle;
    // Start is called before the first frame update
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        g = Mathf.Abs(Physics.gravity.y);
    }

    void Start()
    {
        RenderArc();
    }
    // Update is called once per frame

    void RenderArc()
    {
        lr.positionCount = resolution + 1;
        lr.SetPositions(CalculateArcArray());
    }

    Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[resolution + 1];
        radianAngle = Mathf.Deg2Rad * angle;
        float maxDistance = (vel * vel * Mathf.Sin(2 * radianAngle) / g);
        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t, maxDistance);
        }
        return arcArray;
    }

    Vector3 CalculateArcPoint(float t, float maxDistance)
    {
        float x = t * maxDistance;
        float y = x * Mathf.Tan(radianAngle) - ((g * x * x) / (2 * vel * vel * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
        return new Vector3(x, y);
    }
}
