using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Operator : MonoBehaviour
{

    public GameObject rightClickPanel;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouse = Input.mousePosition;
        if (Input.GetMouseButtonDown(1))
        {
            rightClickPanel.SetActive(true);
            //var mouseCoord = Vector3(Input.mousePosition);
            Debug.Log(Input.mousePosition);
        }
    }
}
