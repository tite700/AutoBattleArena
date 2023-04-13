using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] private GameObject melee;
    [SerializeField] private GameObject ranged;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private GameObject sphereIndicator;


    public int Gold;
    private Ray ray;
    public int meleePrice;
    public int rangedPrice;
    public bool gameOver;
    private SphereIndicator IndicatorScript;
    private Melee MeleeScript;
    private Range RangedScript;
    private bool isGameFrozen = true;

    public bool gameFrozen
    {
        get => isGameFrozen;
        set => isGameFrozen = value;
    }


    //list of gameObjects from the UI that will disappear when the game starts
    [SerializeField] private GameObject[] UIObjects;
    
    //Don't destroy on load
    private void Awake()
    {
        DontDestroyOnLoad(this);
        IndicatorScript = sphereIndicator.GetComponent<SphereIndicator>();
        MeleeScript = melee.GetComponent<Melee>();
        RangedScript = ranged.GetComponent<Range>();
    }
    
    public void SpawnMelee()
    {
        if (Gold >= meleePrice)
        {
            StartCoroutine(SpawnCoroutine(melee,meleePrice));
        }
    }

    public void SpawnRanged()
    {
        if (Gold >= rangedPrice)
        {
            StartCoroutine(SpawnCoroutine(ranged,rangedPrice));
        }
    }

    public void StartGame()
    {
        isGameFrozen = false;
        foreach (var obj in UIObjects)
        {
            obj.SetActive(false);
        }
    }

    IEnumerator SpawnCoroutine(GameObject prefab,int price)
    {
        //Wait for the user to click on the position where he wants to spawn the unit and get the position
        while (!Input.GetMouseButtonDown(0))
        {
            sphereIndicator.SetActive(true);
            yield return null;
        }
       

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {


            Vector3 pointOnNavMesh = default;
            if (NavMesh.SamplePosition(hitInfo.point, out NavMeshHit navMeshHit, 50.0f, NavMesh.AllAreas))
            {
                pointOnNavMesh = navMeshHit.position;
            }

            // Instancie l'objet à l'emplacement du point sur le NavMesh
            if (IndicatorScript.isValid)
            {
                Instantiate(prefab, pointOnNavMesh, Quaternion.identity);
                Gold -= price;
            }
            
        }
        sphereIndicator.SetActive(false);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Make the sphere indicator follow the mouse
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            sphereIndicator.transform.position = hitInfo.point;
            sphereIndicator.transform.position = new Vector3(sphereIndicator.transform.position.x, 0.00001f, sphereIndicator.transform.position.z);
        }


        goldText.text = Gold.ToString();
    }
}
