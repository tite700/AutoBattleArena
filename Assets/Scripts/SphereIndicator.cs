using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereIndicator : MonoBehaviour
{
    [SerializeField] private GameObject ground;
    [SerializeField] private Material redMat;
    [SerializeField] private Material greenMat;

    public bool isValid;

    // Start is called before the first frame update
    void Start()
    {
        // Change sphere's material to green
        GetComponent<Renderer>().material = greenMat;
    }

    // Update is called once per frame
    void Update()
    {
        // Cast a ray downward from the sphere
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            // Check if the ray hit the ground layer
            if (hit.collider.gameObject.layer == ground.layer)
            {
                // Set isValid to true and change sphere's material to green
                isValid = true;
                GetComponent<Renderer>().material = greenMat;
            }
            else
            {
                // Set isValid to false and change sphere's material to red
                isValid = false;
                GetComponent<Renderer>().material = redMat;
            }
        }
    }
}
