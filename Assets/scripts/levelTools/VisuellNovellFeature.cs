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
        bool renderedText = false; 
        dialogcounter = 0;
        while (dialogcounter < DIaloges.Count)
        {
            if (!renderedText) 
            {
                renderedText = true; 
                text.text = string.Empty;
                _textToShow = DIaloges[dialogcounter];
                FadeInText();
            }
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                dialogcounter++;
                renderedText = false;
                Debug.Log("Space"); 
                yield return new WaitForSeconds(time / DIaloges.Count);
            }
            yield return new WaitForSeconds(time / DIaloges.Count);
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
    }
}
