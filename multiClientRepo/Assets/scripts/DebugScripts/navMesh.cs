using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class navMesh : MonoBehaviour
{
    public NavMeshAgent agent;
    private Vector3 destinationPoint;
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit target;

            if(Physics.Raycast(ray, out target))
            {
                agent.destination = target.point;
            }
        }
    }
}
