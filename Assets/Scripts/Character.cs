using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    [SerializeField] protected string enemyTag;

    public string EnemyTag
    {
        get => enemyTag;
        set => enemyTag = value;
    }

    private float _speed = 3f;
    
    private Material _material;
    private Color _color;
    
    
    protected float Cooldown;
    private float _soustract = 100000f;
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
    private float _lastAttackTime = 0f;
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

    private Vector3 GetClosestEnemyPos()
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
    
    protected internal void TakeDamage(float damage)
    {
        
        _hp -= damage;
        Debug.Log(name +" " + _hp);
        foreach (SkinnedMeshRenderer mesh in _tabMesh)
        {
            Material mat = new Material(mesh.materials[0]);
            //Debug.Log("mat " + mat.name);
            mat.color = Color.Lerp(Color.white, mat.color, Mathf.PingPong(Time.time, 1));

        }
        //StartCoroutine(FlashWhite(_timer));

        if (_hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    
    private IEnumerator FlashWhite(float timer)
    {
        while (Time.time - timer <= 1f)
        {
            Debug.Log("boucle");
            _material.color = Color.Lerp(Color.white, _color, Mathf.PingPong(Time.time, 1));
            yield return null;
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
        if (Time.time > Cooldown)
        {
            _soustract = Time.time - _lastAttackTime;
        }
        
        if (distance <= Range )
        {
            MoveToPosition(transform.position);
            if (_soustract >= Cooldown)
            {
                Attack();
                _lastAttackTime = Time.time;
            }
        }
        else
        {
            MoveToPosition(closestEnemy);
        }
    }
}