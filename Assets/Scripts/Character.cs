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
    private SkinnedMeshRenderer[] _tabMesh;


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
        foreach (GameObject enemy in
                 GameObject.FindGameObjectsWithTag(enemyTag)) // penser à utiliser un booléen d'actualisation
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
        if (_tabMesh[0].material.color != Color.white)
        {
            StartCoroutine(FlashWhite());
        }

        var transform1 = transform;

        if (gameObject.CompareTag("Player"))
        {
            Instantiate(blueBlood, transform1.position, transform1.rotation);
        }
        else if (gameObject.CompareTag("Enemy"))
        {
            Instantiate(redBlood, transform1.position, transform1.rotation);
        }
    }


    private IEnumerator FlashWhite()
    {
        for (int i = 0; i < 1; i++)
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


    protected virtual void Attack(GameObject closestEnemy)
    {
    }

    // Update is called once per frame
    protected void Update()
    {
        if (_hp >= 0)
        {
            var closestEnemy = GetClosestEnemy();
            var closestEnemyPos = closestEnemy.transform.position;
            if (closestEnemyPos != Vector3.zero)
            {
                float distance = Vector3.Distance(transform.position, closestEnemyPos);

                if (distance <= Range)
                {
                    MoveToPosition(transform.position);
                    Attack(closestEnemy);
                }
                else
                {
                    MoveToPosition(closestEnemyPos);
                }
            }
            else
            {
                MoveToPosition(Vector3.zero);
            }
        }
    }
}