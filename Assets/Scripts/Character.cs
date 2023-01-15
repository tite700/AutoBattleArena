using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Character : MonoBehaviour
{
    [SerializeField] protected string enemyTag;
    [SerializeField] protected GameObject blueBlood; 
    [SerializeField] protected GameObject redBlood;

    public string EnemyTag
    {
        get => enemyTag;
        set => enemyTag = value;
    }

    private float _speed = 3f;
    
    private Material _material;
    private Color _color;
    
    
    protected float Cooldown;
    //private float _soustract = 100000f;
    private float _timer;
    

    protected float Damage;
    public float damage
    {
        get => Damage;
        set => Damage = value;
    }


    private float _hp;
    public float Hp
    {
        get => _hp;
        set => _hp = value;
    }
    

    protected float Range;
    private NavMeshAgent _navMeshAgent;
    //private float _lastAttackTime = 0f;
    private SkinnedMeshRenderer[] _tabMesh;


    // Start is called before the first frame update
    void Start()
    {

    }

    protected virtual void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _speed;
        _tabMesh = GetComponentsInChildren<SkinnedMeshRenderer>(true);
        
    }

    protected internal GameObject GetClosestEnemy()
    {
        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag(enemyTag)) // penser à utiliser un booléen d'actualisation
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }
    
    protected internal Vector3 GetClosestEnemyPos()
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

    protected virtual void MoveToPosition(Vector3 destination)
    {
        if (_navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.SetDestination(destination);
        }
        
    }
    
    protected internal virtual void TakeDamage(float damage)
    {
        
        _hp -= damage;
        StartCoroutine(FlashWhite());
        var transform1 = transform;
        if (_hp <= 0)
        {
            if (gameObject.CompareTag("Player"))
            {
            
                Instantiate(blueBlood, transform1.position, transform1.rotation);
            }
            else if(gameObject.CompareTag("Enemy"))
            {
                Instantiate(redBlood, transform1.position, transform1.rotation);
            }
        }
    }
    
    
    private IEnumerator FlashWhite()
    {
        for (int i = 0; i < 2; i++)
        {
            foreach (SkinnedMeshRenderer mesh in _tabMesh)
            {
                var material = mesh.material;
                _color = material.color;
                material.color = Color.white;
            }

            yield return new WaitForSeconds(0.15f);
            foreach (SkinnedMeshRenderer mesh in _tabMesh)
            {
                mesh.material.color = _color;
            }
            yield return new WaitForSeconds(0.15f);

        }
    }


    protected virtual void Attack()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        if (_hp >= 0)
        {
            Vector3 closestEnemy = GetClosestEnemyPos();
            if (closestEnemy != Vector3.zero)
            {
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
    }
}