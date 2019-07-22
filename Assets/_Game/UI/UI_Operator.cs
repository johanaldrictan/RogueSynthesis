using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Operator : MonoBehaviour
{

    public GameObject actionPanel;
    public GameObject profilePanel;
    //public GameObject self;
    public GameObject phasePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI statText;
    public TextMeshProUGUI phaseText;
    public GameObject flavorText;
    private int randomIndex;

    public void Start()
    {
        PhaseTextDisplay();
    }

    public void SetTextInfo(int health, string name, int moveSpeed)
    {
        healthText.text = "Health: " + health;
        nameText.text = name;
        statText.text = "Move Speed: " + moveSpeed;
    }

    public void SetPhaseText(int playerID)
    {
        if (playerID == 0)
        {
            randomIndex = Random.Range(0, flavorText.GetComponent<flavorText>().EnemySentences.Length);
            phaseText.text = flavorText.GetComponent<flavorText>().EnemySentences[randomIndex];

        }
        else if (playerID == 1)
        {
            randomIndex = Random.Range(0, flavorText.GetComponent<flavorText>().PlayerSentences.Length);
            phaseText.text = flavorText.GetComponent<flavorText>().PlayerSentences[randomIndex];
        }
    }

    public void InitializeUI()
    {
        profilePanel.SetActive(true);
    }

    public void PhaseTextDisplay()
    {
        StartCoroutine(PhaseDisplay());
    }

    public IEnumerator PhaseDisplay()
    {
        phasePanel.SetActive(true);
        yield return new WaitForSeconds(2);
        phasePanel.SetActive(false);
    }




    //public bool mouseOnObject;

    /*private void OnMouseEnter()
    {
        mouseOnObject = true;
    }

    private void OnMouseExit()
    {
        mouseOnObject = false;
    }*/

    // Update is called once per frame
    //void Update()
    //{
        //Debug.Log(GetComponent<Unit>().GetName()) ;
        //Debug.Log(GetComponent<UnitController>().getActiveUnit());
        /*
        Vector3 mouse = Input.mousePosition;
        if (Input.GetMouseButtonDown(1))
        {
            actionPanel.SetActive(true);
            //var mouseCoord = Vector3(Input.mousePosition);
            Debug.Log(Input.mousePosition);
        }

        if (this.name == "Unit-stats Pop-up Window" && Input.GetMouseButtonDown(0) && !mouseOnObject)
        {
            self.SetActive(false);
        }*/
    //}
}
