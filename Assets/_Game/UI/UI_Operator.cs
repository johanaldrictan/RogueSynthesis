using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Operator : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject actionPanel;
    public GameObject profilePanel;
    //public GameObject self;
    public GameObject phasePanel;

    [Header("Animators")]
    public Animator phaseAnimator;

    [Header("Unit Information")]
    public Unit unit;
    public Image portrait;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI statText;

    [Header("Miscellaneous Text")]
    public TextMeshProUGUI phaseText;
    public GameObject flavorText;
    public TextMeshProUGUI ability1;
    public TextMeshProUGUI ability2;

    private int randomIndex;

    public void Start()
    {
        PhaseTextDisplay();
    }

    public void SetInfo()
    {
        healthText.text = "Health: " + unit.GetHealth();
        nameText.text = unit.GetName();
        statText.text = "Move Speed: " + unit.GetMoveSpeed();
        portrait.sprite = unit.GetPortrait();
        ability1.text = unit.AvailableAbilities[1].ToString();
        ability2.text = unit.AvailableAbilities[2].ToString();
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

        //phasePanel.SetActive(true);
        //Debug.Log("bbbb");
        yield return new WaitForSeconds(2);
        phaseAnimator.SetTrigger("New Trigger");
        //Debug.Log("asdf");
        //phasePanel.SetActive(false);
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
