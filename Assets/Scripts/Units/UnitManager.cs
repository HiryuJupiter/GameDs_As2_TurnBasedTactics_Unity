using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Layout;

public class UnitManager : MonoBehaviour
{
    public Camera cam;
    public GameObject[] units = new GameObject[7];
    public bool human;
    public Material highlight;
    public Material defaultTeam;


    // Start is called before the first frame update
    void Start()
    {

    }
    IEnumerator SpawnMode(int unitIndex)
    {
        bool looping = true;

        while (looping)
        {

            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                GameObject currentObj = hit.transform.gameObject;
                Hex currentHex = currentObj.GetComponent<Hex>();

                if (currentHex.blue)
                {

                    currentObj.GetComponent<Renderer>().material = highlight;

                }



            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                looping = false;
                yield return null;
               
                    
            }
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(units[unitIndex], transform.position, transform.rotation);
                looping = false;
                yield return null;
                
            }
            yield return null;
        }


    }


    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {

            StartCoroutine(SpawnMode(0));
        }
#endif
    }


}
