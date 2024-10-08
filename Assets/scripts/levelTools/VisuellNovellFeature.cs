using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class VisuellNovellFeature : MonoBehaviour
{
    [SerializeField]
    private string _textToShow = "";
    private bool wait; 
    public TextMeshProUGUI text;
    public List<string> DIaloges = new List<string>();
    private int dialogcounter;

    private void Start()
    {
        StartCoroutine(Dialog(1)); 
    }

    private IEnumerator Dialog(float time)
    {
        dialogcounter = 0;
        while (dialogcounter < DIaloges.Count)
        {
            text.text = string.Empty;
            _textToShow = DIaloges[dialogcounter];
            FadeInText();
            yield return new WaitForSeconds(time/ DIaloges.Count);
        }

    }
    void FadeInText()
    {
        StartCoroutine(AddCharOverTime(0.2f));
    }

    private IEnumerator AddCharOverTime(float time)
    {
        string currentText = "";
        foreach (char c in _textToShow)
        {
            currentText += c;
            text.text = currentText;
            yield return new WaitForSeconds(time);
        }
        dialogcounter++;
    }
}
