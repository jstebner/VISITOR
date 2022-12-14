using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLineOfSight : MonoBehaviour
{

    public Transform visitor;
    public Visitor visitorScript;
    public Transform playerCamera;
    [SerializeField] private float sightCone;
    [SerializeField] private float maxLookingAwayTime;
    private float lookingAwayTime = 0;

    [SerializeField] private float maxLineOfSightTime;
    private float lineOfSightTime = 0;

    [SerializeField] private GameObject randomWaypoint;

    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (visitor == null) {
            return;
        }

        if (hasLineOfSightWithVisitor()) {
            lineOfSightTime += Time.deltaTime;
        }
        if (lineOfSightTime >= maxLineOfSightTime && visitorScript.getState() == Visitor.State.Patrol) {
            visitorScript.setState(Visitor.State.TargetPlayer);
            lineOfSightTime = 0;
            lookingAwayTime = 0;
        }
        if (visitorScript.getState() == Visitor.State.TargetPlayer) {
            Vector3 dirFromPlayerToVisitor = (visitor.position - transform.position).normalized;        
            float dotProduct = Vector3.Dot(dirFromPlayerToVisitor, playerCamera.forward);
            if (dotProduct < sightCone) {
                lookingAwayTime += Time.deltaTime;
            } else {
                lookingAwayTime -= Time.deltaTime;
                lookingAwayTime = Mathf.Clamp(lookingAwayTime, 0, maxLookingAwayTime);
            }
            if (lookingAwayTime >= maxLookingAwayTime) {
                //Debug.Log("Visitor Waiting Because player looked away");
                visitorScript.setState(Visitor.State.Waiting);
                lineOfSightTime = 0;
                lookingAwayTime = 0;
            }
        }
    }

    public void resetLookTime() {
        lookingAwayTime = 0;
    }

    public bool hasLineOfSightWithWaypoint(Transform obj) {
        Vector3 dirFromPlayerToObject = (obj.position - transform.position).normalized;
        float maxDistance = Vector3.Distance(transform.position, obj.position);
        return !Physics.Raycast(transform.position, dirFromPlayerToObject, out RaycastHit hitInfo, maxDistance);
    }

    private bool hasLineOfSightWithVisitor() {
        Vector3 dirFromPlayerToObject = (visitor.position - transform.position).normalized;
        Physics.Raycast(transform.position, dirFromPlayerToObject, out RaycastHit hitInfo);
        return hitInfo.transform.name == "VisitorPrefab";
    }
}
