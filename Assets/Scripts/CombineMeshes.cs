using UnityEngine;

public class CombineMeshes : MonoBehaviour
{
    protected void Start()
    {
        StaticBatchingUtility.Combine(gameObject);
    }
}