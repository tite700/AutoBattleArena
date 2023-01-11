using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    [SerializeField] private string enemyTag;
    
    protected float Speed = 3f;
    protected float Hp;
    protected float Damage;
    protected float Range;
    private NavMeshAgent _navMeshAgent;
    protected float Cooldown;


    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = Speed;
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