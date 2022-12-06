using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLineOfSight : MonoBehaviour
{

    public Transform visitor;
    public Visitor visitorScript;
    public Transform playerCamera;
    [SerializeField] private float sightCone;
    [SerializeField] private float maxLookTime;
    private float lookTime;

    
    // Update is called once per frame
    void Update()
    {
        if (visitor != null) {
            Vector3 dirFromPlayerToVisitor = (visitor.position - transform.position).normalized;
            float dotProduct = Vector3.Dot(dirFromPlayerToVisitor, playerCamera.forward);
            if (dotProduct > sightCone) {
                Physics.Raycast(transform.position, dirFromPlayerToVisitor, out RaycastHit hitInfo);
                if (hitInfo.transform.name == "VisitorPrefab") {
                    lookTime += Time.deltaTime;
                } else {
                    lookTime = 0;
                }
            } else {
                lookTime = 0;
            }
            if (lookTime >= maxLookTime && visitorScript.getState() == Visitor.State.Patrol) {
                visitorScript.setState(Visitor.State.TargetPlayer);
            }
        }
    }
}
