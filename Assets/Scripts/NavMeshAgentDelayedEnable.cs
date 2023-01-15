using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Ce script permet d'éviter des warnings dans le cas où
/// le NavMeshAgent est actif alors que le NavMesh n'est
/// pas encore généré.
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