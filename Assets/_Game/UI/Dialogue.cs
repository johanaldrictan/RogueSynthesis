using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public LevelChanger levelChanger;
    public Animator animator;

    public Image portrait;
    public TextMeshProUGUI nameDisplay;
    public TextMeshProUGUI textDisplay;

    public Dialog[] dialogArray;

    private int index;
    public float typingSpeed = 0.02f;
    //private bool complete;

    void Start()
    {
        portrait.sprite = dialogArray[index].portrait;
        nameDisplay.text = dialogArray[index].name;
        textDisplay.text = "";
        StartCoroutine(Type());
    }

    void Update()
    {
        if (textDisplay.text == dialogArray[index].sentence && Input.GetKeyDown("space"))
        {
            NextSentence();
        }
    }

    IEnumerator Type()
    {
        foreach (char letter in dialogArray[index].sentence)
        {
            textDisplay.text += letter;
            if (Input.GetKey(KeyCode.Space))
            {
                yield return new WaitForSeconds(0.001f);
            }
            else
            {
                yield return new WaitForSeconds(typingSpeed);
            }
            
        }
    }

    public void NextSentence()
    {
        if (index < dialogArray.Length - 1)
        {
            index++;
            portrait.sprite = dialogArray[index].portrait;
            nameDisplay.text = dialogArray[index].name;
            textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            Proceed();
        }
    }

    public void Proceed()
    {
        portrait.sprite = null;
        nameDisplay.text = "";
        textDisplay.text = "";
        //complete = true;
        //Debug.Log("Next");
        animator.SetTrigger("New Trigger");
        if (levelChanger != null)
        {
            levelChanger.GetComponent<LevelChanger>().FadeToNextLevel();
        }
    }

}
