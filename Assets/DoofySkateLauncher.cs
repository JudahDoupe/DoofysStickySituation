using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoofySkateLauncher : MonoBehaviour
{

    public GameObject Doofy;

    private CapsuleCollider DoofCol;
    private MeshCollider thisCol;

    private bool launch;

    // Start is called before the first frame update
    void Start()
    {
        launch = false;
        DoofCol = Doofy.GetComponent<CapsuleCollider>();
        thisCol = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter (Collider col)
    {
        Debug.Log("Hit");
        if ((col.name == "Book" || col.gameObject.name == "Book") && !launch)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            launch = true;
            Physics.IgnoreCollision(DoofCol, thisCol, true);
            Doofy.GetComponent<Launch>().startLaunch();
        }
    }
}
