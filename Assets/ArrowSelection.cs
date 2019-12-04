using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSelection : MonoBehaviour
{

    public static int directionID;

    RaycastHit2D hit;

    GameObject arrow1;
    GameObject arrow2;
    GameObject arrow3;
    GameObject arrow4;

    // Start is called before the first frame update
    void Start()
    {
        arrow1 = GameObject.Find("Arrow 1");
        arrow2 = GameObject.Find("Arrow 2");
        arrow3 = GameObject.Find("Arrow 3");
        arrow4 = GameObject.Find("Arrow 4");
        //Component[] children = GetCompon
    }


    private void OnMouseEnter()
    {
        if (hit.collider != null)
        {
            switch (hit.collider.gameObject.name)
            {
                case "Arrow 1":
                    //Debug.Log("test");
                    hit.collider.GetComponent<SpriteRenderer>().color = new Color(0.2588235f, 1, 0.3583538f, 1);
                    break;
                case "Arrow 2":
                    hit.collider.GetComponent<SpriteRenderer>().color = new Color(0.2588235f, 1, 0.3583538f, 1);
                    break;
                case "Arrow 3":
                    hit.collider.GetComponent<SpriteRenderer>().color = new Color(0.2588235f, 1, 0.3583538f, 1);
                    break;
                case "Arrow 4":
                    hit.collider.GetComponent<SpriteRenderer>().color = new Color(0.2588235f, 1, 0.3583538f, 1);
                    break;
            }
        }
        
    }

    private void OnMouseExit()
    {
        arrow1.GetComponent<SpriteRenderer>().color = new Color(0.259434f, 0.7345216f, 1, 1);
        arrow2.GetComponent<SpriteRenderer>().color = new Color(0.259434f, 0.7345216f, 1, 1);
        arrow3.GetComponent<SpriteRenderer>().color = new Color(0.259434f, 0.7345216f, 1, 1);
        arrow4.GetComponent<SpriteRenderer>().color = new Color(0.259434f, 0.7345216f, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
            

        //}

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider != null)
        {
            //Debug.Log(hit.collider.gameObject.name);
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.gameObject.name == "Arrow 1")
                {
                    directionID = 1;
                }
                else if (hit.collider.gameObject.name == "Arrow 2")
                {
                    directionID = 2;
                }
                else if (hit.collider.gameObject.name == "Arrow 3")
                {
                    directionID = 3;
                }
                else if (hit.collider.gameObject.name == "Arrow 4")
                {
                    directionID = 4;
                }
            }
            
        }
        /*
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
        */
    }
}
