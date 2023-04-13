using System.Collections;
using TMPro;
using UnityEngine;

public class Tutorial_UI : MonoBehaviour
{
    [SerializeField] private GameObject[] UIObjects;
    [SerializeField] private string[] tutorialText;
    [SerializeField] private GameObject backgroundText;
    [SerializeField] private TMP_Text tutorialTextWindow;

    private int indexArrow = -1;
    private int indexText = 0;

    // Start is called before the first frame update
    void Start()
    {
        tutorialTextWindow.text = tutorialText[0];
        indexText = 1;
    }


    IEnumerator Blink(GameObject obj)
    {
        int tmp = indexArrow;
        while (tmp == indexArrow)
        {
            obj.SetActive(false);
            yield return new WaitForSeconds(0.35f);
            obj.SetActive(true);
            yield return new WaitForSeconds(0.35f);
        }
        obj.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
    
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            indexArrow++;
            if (indexArrow < UIObjects.Length)
            {
                tutorialTextWindow.text = tutorialText[indexText];
                indexText++;
                UIObjects[indexArrow].SetActive(true);
                StartCoroutine(Blink(UIObjects[indexArrow]));
                
            }
            else
            {
                indexArrow ++;
                tutorialTextWindow.text = "";
                backgroundText.SetActive(false);
                foreach (var obj in UIObjects)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}
