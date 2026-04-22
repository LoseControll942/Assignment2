using UnityEngine;
using UnityEngine.AI;
public class NPCLocomotion : MonoBehaviour
{
    [SerializeField] Transform[] destination;
    NavMeshAgent navMeshAgent;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("The NavMeshAgent component is not attached to "
            + gameObject.name);
        }
        else
        {
            SetDestination();
        }
    }
    void SetDestination()
    {
        int selector = Random.Range(0, destination.Length);
        if (destination != null)
        {
            Vector3 targetVector = destination[selector].position;
            navMeshAgent.SetDestination(targetVector);
        }
    }

    private void Update()
    {
        if(!navMeshAgent.isOnNavMesh)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            return;
        }
        if(!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            SetDestination();
        }
    }
}
