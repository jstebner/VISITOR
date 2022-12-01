using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class visitorMove : MonoBehaviour
{

    public GameObject point;
    private NavMeshAgent nav;
    // Start is called before the first frame update
    void Start()
    {
        nav = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(point.transform.position);
    }
}
