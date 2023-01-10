using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Ce script permet d'�viter des warnings dans le cas o�
/// le NavMeshAgent est actif alors que le NavMesh n'est
/// pas encore g�n�r�.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshAgentDelayedEnable : MonoBehaviour
{
    protected void Awake()
    {
        var navMeshAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(Coroutine());
        IEnumerator Coroutine()
        {
            navMeshAgent.enabled = false;
            yield return null;
            navMeshAgent.enabled = true;
            Destroy(this);
        }
    }
}
