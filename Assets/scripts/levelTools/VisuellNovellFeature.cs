using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class VisuellNovellFeature : MonoBehaviour
{
    public TextMeshProUGUI text;
    public List<string> DIaloges = new List<string>();

    [SerializeField]
    private string _textToShow = "";
    private bool _wait;
    private bool renderedText;
    private int _dialogcounter;
    private bool _isActive = false;
    private bool _continue = false; 
    
    private void Start()
    {
        StartCoroutine(Dialog(1)); 
    }
    private void Update()
    {
        if(_isActive)
        {
            //to-do Entferne diese Input logik, wenn die Input Mappings für das neue InputSystem Existiteren
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                _continue = true; 
            }

            if (_continue) 
            {
                _continue = false;
                _dialogcounter++;
                renderedText = false;
                Debug.Log("Space");
            }
        }
    }
    private IEnumerator Dialog(float time)
    {
        _isActive=true; 
         renderedText = false; 
        _dialogcounter = 0;
        while (_dialogcounter < DIaloges.Count)
        {
            if (!renderedText) 
            {
                renderedText = true; 
                text.text = string.Empty;
                _textToShow = DIaloges[_dialogcounter];
                FadeInText();
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
