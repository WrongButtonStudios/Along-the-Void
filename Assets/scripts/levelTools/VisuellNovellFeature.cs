using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;

public class VisuellNovellFeature : MonoBehaviour
{

    private enum Step 
    {
        FadeInSpeechBubble, 
        FadeInText, 
        FadeOutSpeechBubble
    }
    public TextMeshPro text;
    public List<string> Dialoges = new List<string>();

    [SerializeField]
    private string _textToShow = "";
    [SerializeField]
    private GameObject TextPref;
    [SerializeField]
    private Vector3 _desiredSize = new Vector3(1, 1, 1); 
    private bool _wait;
    private bool renderedText;
    private int _dialogcounter;
    private bool _isActive = false;
    private bool _continue = false;
    private Step _currentStep = Step.FadeInSpeechBubble;
    private bool runDialog;
    private bool inCourountine = false;

    public void StartDialog(List<string> texts)
    {
        Dialoges.Clear();
        Dialoges = texts;
        _dialogcounter = 0; 
        _currentStep = Step.FadeInSpeechBubble;
        runDialog = true;
    }
    private void DialogStateHandle() 
    {
        _isActive = true; 
        if (runDialog) 
        {
            switch (_currentStep) 
            {
                case Step.FadeInSpeechBubble:
                    FadeIn();
                    break;
                case Step.FadeInText:
                    StartCoroutine(Dialog(1));
                    break;
                case Step.FadeOutSpeechBubble:
                    //TextPref.SetActive(false);
                    runDialog = false;
                    _isActive = false; 
                    return; 
            }
        
        }
    }

    public void ResetDialog() 
    {
        _dialogcounter = 0;
        _currentStep = Step.FadeInText; 
    }
    private void Update()
    {
        if(_isActive)
        {
            //to-do Entferne diese Input logik, wenn die Input Mappings für das neue InputSystem Existieren
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
        if(!inCourountine)
            DialogStateHandle();
    }


    private IEnumerator Dialog(float time)
    {
        inCourountine = true; 
        _isActive=true; 
         renderedText = false; 
        _dialogcounter = 0;
        while (_dialogcounter < Dialoges.Count)
        {
            if (!renderedText) 
            {
                renderedText = true; 
                text.text = string.Empty;
                _textToShow = Dialoges[_dialogcounter];
                FadeInText();
            }
            yield return new WaitForSeconds(time / Dialoges.Count);
        }
        _currentStep = Step.FadeOutSpeechBubble;
        inCourountine = false; 
    }
    void FadeInText()
    {
        StartCoroutine(AddCharOverTime(0.2f));
    }

    private IEnumerator AddCharOverTime(float time)
    {
        inCourountine = true; 
        string currentText = "";
        foreach (char c in _textToShow)
        {
            currentText += c;
            text.text = currentText;
            yield return new WaitForSeconds(time);
        }

    }

    private void FadeIn() 
    {
        TextPref.transform.localScale = TextPref.transform.localScale / 10;
        StartCoroutine(FadeInYield()); 
    }

    IEnumerator FadeInYield() 
    {
        inCourountine = true; 
        while(TextPref.transform.localScale.sqrMagnitude < _desiredSize.sqrMagnitude) 
        {
            Vector3 addScale = new Vector3(Time.fixedDeltaTime, Time.fixedDeltaTime, Time.fixedDeltaTime);
            TextPref.transform.localScale += addScale; 
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        _currentStep = Step.FadeInText;
        inCourountine = false; 
    }
}
