using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    public Camera cam;
    NavMeshAgent agent;
    public LayerMask ground;
    public bool isCommandedToMove;
    DirectionIndicator directionIndicator;
    private float originalStoppingDistance;

    private void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        directionIndicator = GetComponent<DirectionIndicator>();
        if (agent != null)
        {
            originalStoppingDistance = agent.stoppingDistance;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                float distanceToTarget = Vector3.Distance(transform.position, hit.point);
                
                // If clicking very close to the unit, use a smaller stopping distance
                if (distanceToTarget < originalStoppingDistance * 2)
                {
                    agent.stoppingDistance = 0.1f;
                }
                else
                {
                    agent.stoppingDistance = originalStoppingDistance;
                }
                
                isCommandedToMove = true;
                agent.SetDestination(hit.point);
                directionIndicator.DrawLine(hit);
            }
        }

        if (agent.hasPath == false || agent.remainingDistance < agent.stoppingDistance)
        {
            isCommandedToMove = false;
            // Reset stopping distance when movement is complete
            agent.stoppingDistance = originalStoppingDistance;
        }
    }
}
