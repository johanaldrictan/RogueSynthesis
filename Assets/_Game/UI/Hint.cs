using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hint : MonoBehaviour
{
    public TextMeshProUGUI hintText;

    [TextArea(3, 10)]
    public string[] sentences;
    private int index;

    private int randomizer;

    public void Start()
    {
        randomizer = Random.Range(0, sentences.Length);
        hintText.text = sentences[randomizer];
    }


}
