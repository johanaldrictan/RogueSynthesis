using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSelection : MonoBehaviour
{

    //private GameObject arrow1, arrow2, arrow3, arrow4;

    RaycastHit ObjectHitByMouse;

    // Start is called before the first frame update
    void Start()
    {
        //arrow1 = GameObject.Find("Arrow 1");
        //arrow2 = GameObject.Find("Arrow 2");
        //arrow3 = GameObject.Find("Arrow 3");
        //arrow4 = GameObject.Find("Arrow 4");
        //Component[] children = GetCompon
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Debug.Log(ObjectHitByMouse.collider.gameObject);

        if (Physics.Raycast(ray.origin, ray.direction, out ObjectHitByMouse, 2000))
        {
            if (ObjectHitByMouse.collider.gameObject.name == "Arrow 1")
            {
                Debug.Log("Im over 'This'");
            }

            if (ObjectHitByMouse.collider.gameObject.name == "Arrow 2")
            {
                Debug.Log("Im over 'That'");
            }

        }
    }
}
