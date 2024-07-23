using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{

    private bool fadeOut, fadeIn;
    public float fadeSpeedIn;
    public float fadeSpeedOut;
    public float spawnCoolDown = 3f;

    [SerializeField] private Image objectToFade;

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //if (fadeOut)
        //{
        //    Color objectColor = objectToFade.color;
        //    float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

        //    objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
        //    objectToFade.material.color = objectColor;

        //    if (objectColor.a <= 0)
        //    {
        //        fadeOut = false;
        //    }
        //}
    }

    public void FadeScreen(bool isFadingIn, PlayerController playerController)
    {
        if (isFadingIn)
        {
            StartCoroutine(FadeInObject(playerController));
        }
        else
        {
            StartCoroutine(FadeOutObject());
        }
    }

    public IEnumerator FadeInObject(PlayerController playerController)
    {
        while (objectToFade.color.a < 1)
        {
            Color objectColor = objectToFade.color;
            float fadeAmount = objectColor.a + (fadeSpeedIn * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            objectToFade.color = objectColor;
            yield return null;/*new WaitForSeconds(spawnCoolDown);*/
        }
        yield return new WaitForSeconds(spawnCoolDown);
        playerController.Spawn();
        StartCoroutine(FadeOutObject());

    }
    public IEnumerator FadeOutObject()
    {
        //yield return new WaitForSeconds(1);
        SoundManager.sndMan.PlayRespawnSound();
        Debug.Log("fired");
        while (objectToFade.color.a > 0)
        {
            Color objectColor = objectToFade.color;
            float fadeAmount = objectColor.a - (fadeSpeedOut * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            objectToFade.color = objectColor;
            yield return null;
        }
    }

}