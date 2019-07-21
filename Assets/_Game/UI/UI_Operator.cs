using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Operator : MonoBehaviour
{

    public GameObject rightClickPanel;
    public GameObject self;

    public bool mouseOnObject;

    private void OnMouseEnter()
    {
        mouseOnObject = true;
    }

    private void OnMouseExit()
    {
        mouseOnObject = false;
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

        if (this.name == "Unit-stats Pop-up Window" && Input.GetMouseButtonDown(0) && !mouseOnObject)
        {
            self.SetActive(false);
        }
    }
}
