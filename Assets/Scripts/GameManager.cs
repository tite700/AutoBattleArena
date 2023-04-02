using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] private GameObject melee;
    
    //Don't destroy on load
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    
    public void SpawnMelee()
    {
        StartCoroutine(SpawnCoroutine(melee));
    }

    IEnumerator SpawnCoroutine(GameObject prefab)
    {
        //Wait for the user to click on the position where he wants to spawn the unit and get the position
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            // Affiche un rayon partant de la souris jusqu'au point d'impact
            Debug.DrawRay(ray.origin, hitInfo.point - ray.origin, Color.red, 10.0f);
            Debug.Log(hitInfo.point);

            // Dessine un petit point sur le sol à l'emplacement où l'objet sera placé
            Vector3 pointOnNavMesh = default;
            if (NavMesh.SamplePosition(hitInfo.point, out NavMeshHit navMeshHit, 50.0f, NavMesh.AllAreas))
            {
                pointOnNavMesh = navMeshHit.position;
                Debug.DrawRay(pointOnNavMesh, Vector3.up, Color.green, 10.0f);
            }

            // Instancie l'objet à l'emplacement du point sur le NavMesh
            Instantiate(prefab, pointOnNavMesh, Quaternion.identity);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
