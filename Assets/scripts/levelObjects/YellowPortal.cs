using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowPortal : MonoBehaviour
{
    [Header("Portal Settings")]
    public bool isTeleporting = false; // False = Cannon Portal, True = Teleport Portal
    [SerializeField] private GameObject linkedPortal;
    [SerializeField] private float cooldown = 1f;

    [Header("Cannon Portal Settings")]
    [SerializeField] private float cannonHoldTime = 2f;
    [SerializeField] private float ejectionForce = 10f;
    [SerializeField] private Vector2 ejectionDirection = Vector2.right;

    private bool canTeleport = true;
    private Rigidbody2D playerRb;
    private SpriteRenderer spriteRenderer;
    private Vector3 originalPlayerScale;
    private characterController playerController;

    void Start()
    {
        if (isTeleporting && linkedPortal == null)
        {
            Debug.LogError($"Teleport-Portal {gameObject.name} hat kein verbundenes Portal!");
        }

        // Überprüfe die bidirektionale Verbindung
        if (isTeleporting && linkedPortal != null)
        {
            YellowPortal linkedScript = linkedPortal.GetComponent<YellowPortal>();
            if (linkedScript != null && linkedScript.linkedPortal != gameObject)
            {
                Debug.LogWarning($"Portal {gameObject.name} hat eine einseitige Verbindung zu {linkedPortal.name}. Stelle bidirektionale Verbindung her...");
                linkedScript.linkedPortal = gameObject;
            }
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private bool IsPlayerYellow(GameObject player)
    {
        if (playerController == null)
        {
            playerController = player.GetComponent<characterController>();
        }

        if (playerController != null)
        {
            var playerStatus = playerController.getPlayerStatus();
            return playerStatus.currentState == characterController.playerStates.yellow ||
                   playerStatus.currentState == characterController.playerStates.burntYellow;
        }

        return false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player") || !canTeleport)
        {
            return;
        }

        // Prüfe ob der Spieler gelb ist
        if (!IsPlayerYellow(collider.gameObject))
        {
            // Optional: Visuelles/Audio Feedback dass das Portal nicht reagiert
            Debug.Log("Portal reagiert nicht - Spieler ist nicht gelb!");
            return;
        }

        playerRb = collider.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            originalPlayerScale = collider.transform.localScale;

            if (isTeleporting)
            {
                TeleportToLinkedPortal();
            }
            else
            {
                StartCoroutine(CannonPortalSequence(collider.gameObject));
            }
        }
    }

    private void TeleportToLinkedPortal()
    {
        if (linkedPortal != null)
        {
            YellowPortal linkedPortalScript = linkedPortal.GetComponent<YellowPortal>();
            if (linkedPortalScript != null)
            {
                // Beide Portale deaktivieren
                canTeleport = false;
                linkedPortalScript.canTeleport = false;

                // Teleportiere den Spieler
                Vector2 teleportDirection = linkedPortal.transform.right * (playerRb.velocity.magnitude);


                playerRb.position = linkedPortal.transform.position;
                playerRb.velocity = teleportDirection;

                // Starte Cooldown für beide Portale
                StartCoroutine(StartCooldown());
                StartCoroutine(linkedPortalScript.StartCooldown());
            }
        }
    }

    private IEnumerator CannonPortalSequence(GameObject player)
    {
        // Kontinuierliche Überprüfung des Zustands während der Cannon-Sequenz
        if (!IsPlayerYellow(player))
        {
            yield break;
        }

        canTeleport = false;

        playerRb.velocity = Vector2.zero;
        playerRb.isKinematic = true;

        StartCoroutine(ScalePlayerOverTime(player, Vector3.zero, 0.5f));

        yield return new WaitForSeconds(cannonHoldTime);

        // Finale Überprüfung vor dem Herausschießen
        if (!IsPlayerYellow(player))
        {
            // Stelle Original-Zustand wieder her
            playerRb.isKinematic = false;
            StartCoroutine(ScalePlayerOverTime(player, originalPlayerScale, 0.5f));
            yield break;
        }

        StartCoroutine(ScalePlayerOverTime(player, originalPlayerScale, 0.5f));
        playerRb.isKinematic = false;

        Vector2 normalizedDirection = transform.TransformDirection(ejectionDirection).normalized;
        // Hier verwenden wir AddForce statt velocity
        playerRb.AddForce(normalizedDirection * ejectionForce, ForceMode2D.Impulse);

        StartCoroutine(StartCooldown());
    }

    private IEnumerator ScalePlayerOverTime(GameObject player, Vector3 targetScale, float duration)
    {
        Vector3 startScale = player.transform.localScale;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;
            player.transform.localScale = Vector3.Lerp(startScale, targetScale, progress);
            yield return null;
        }

        player.transform.localScale = targetScale;
    }

    private IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        canTeleport = true;
    }

    public void LinkPortal(GameObject newPortal)
    {
        if (newPortal != null)
        {
            YellowPortal otherPortal = newPortal.GetComponent<YellowPortal>();
            if (otherPortal != null)
            {
                // Stelle bidirektionale Verbindung her
                linkedPortal = newPortal;
                otherPortal.linkedPortal = gameObject;

                // Stelle sicher, dass beide Portale im Teleport-Modus sind
                isTeleporting = true;
                otherPortal.isTeleporting = true;

                Debug.Log($"Bidirektionale Verbindung hergestellt zwischen {gameObject.name} und {newPortal.name}");
            }
        }
    }

    public void SetEjectionDirection(Vector2 newDirection)
    {
        ejectionDirection = newDirection.normalized;
    }
}