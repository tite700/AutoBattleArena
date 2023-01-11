using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    [SerializeField] protected string enemyTag;

    private float _speed = 3f;
    private float _hp;

    public float Hp
    {
        get => _hp;
        set => _hp = value;
    }

    protected float Damage;
    protected float Range;
    private NavMeshAgent _navMeshAgent;
    protected float Cooldown;


    // Start is called before the first frame update
    void Start()
    {

    }

    protected void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _speed;
    }

    protected Vector3 GetClosestEnemyPos()
    {
        Vector3 closestEnemyPos = Vector3.zero;
        float closestDistance = float.MaxValue;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(enemyTag)) // penser à utiliser un booléen d'actualisation
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
    
    protected internal void TakeDamage(float damage)
    {
        _hp -= damage;
        if (_hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    protected virtual void Attack()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        Vector3 closestEnemy = GetClosestEnemyPos();
        float distance = Vector3.Distance(transform.position, closestEnemy);
        if (distance <= Range)
        {
            MoveToPosition(transform.position);
            Attack();
        }
        else
        {
            MoveToPosition(closestEnemy);
        }
    }
}