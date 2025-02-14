using UnityEngine;
using System.Collections;

public class RedShrinkZone : MonoBehaviour
{
    [SerializeField] private Transform player;
    public float shrinkFactor = 0.1f;
    public float shrinkDuration = 2f;
    public float growDuration = 2f;
    public float triggerDelay = 0.5f;  // Verzögerung vor dem Schrumpfen

    private Vector3 originalScale;
    private Coroutine activeCoroutine;
    private bool isInTrigger = false;  // Tracking ob der Spieler im Trigger ist

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is missing!");
            enabled = false;
            return;
        }
        originalScale = player.localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInTrigger = true;
            if (activeCoroutine != null) StopCoroutine(activeCoroutine);
            activeCoroutine = StartCoroutine(DelayedShrink());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInTrigger = false;
            if (activeCoroutine != null) StopCoroutine(activeCoroutine);
            activeCoroutine = StartCoroutine(ChangeScale(originalScale, growDuration));
        }
    }

    private IEnumerator DelayedShrink()
    {
        yield return new WaitForSeconds(triggerDelay);

        // Prüfen ob der Spieler noch im Trigger ist
        if (isInTrigger)
        {
            StartCoroutine(ChangeScale(originalScale * shrinkFactor, shrinkDuration));
        }
    }

    private IEnumerator ChangeScale(Vector3 targetScale, float duration)
    {
        Vector3 startScale = player.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            player.localScale = Vector3.Lerp(startScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        player.localScale = targetScale;
    }
}