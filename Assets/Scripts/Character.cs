using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    [SerializeField] private string enemyTag;
    
    private float _speed = 3f;
    private float _hp;
    private float _damage;
    private float _range;
    private NavMeshAgent _navMeshAgent;


    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _speed;
    }

    private Vector3 GetClosestEnemyPos()
    {
        Vector3 closestEnemyPos = Vector3.zero;
        float closestDistance = float.MaxValue;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(enemyTag))
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemyPos = enemy.transform.position;
            }
        }
        Debug.Log("enemy pos : " + closestEnemyPos);
        return closestEnemyPos;
    }

    private void MoveToPosition(Vector3 destination)
    {
        if (_navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.SetDestination(destination);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPosition(GetClosestEnemyPos());
    }
}