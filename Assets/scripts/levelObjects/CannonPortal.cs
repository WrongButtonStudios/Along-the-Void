using UnityEngine;
using System.Collections;

public class CannonPortal : MonoBehaviour
{
    [SerializeField] private float cannonHoldTime = 2f;
    [SerializeField] private float ejectionForce = 10f;
    [SerializeField] private Vector2 ejectionDirection;
    [SerializeField] private float cooldown = 1f;
    [SerializeField]private characterController playerController;
    private GameObject ps_YellowPortal_Startup_Star;
    private GameObject ps_YellowPortal_FizzleOut;

    private bool canTeleport = true;
    private Rigidbody2D playerRb;
    private Vector3 originalPlayerScale;



    void Awake()
    {
        ps_YellowPortal_Startup_Star = transform.Find("PS_YellowPortal_Startup_Star").gameObject;
        ps_YellowPortal_FizzleOut = transform.Find("PS_YellowPortal_FizzleOut").gameObject;
    }

    void Update()
    {
        if (PlayerUttillitys.GetPlayerColor(playerController) == PlayerColor.yellow)
        {
            this.GetComponent<Collider2D>().enabled = true;
            ps_YellowPortal_Startup_Star.SetActive(true);
            ps_YellowPortal_FizzleOut.SetActive(false);
        }
        else
        {
            this.GetComponent<Collider2D>().enabled = false;
            ps_YellowPortal_Startup_Star.SetActive(false);
            ps_YellowPortal_FizzleOut.SetActive(true);
        }
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
            return playerStatus.currentState == characterController.playerStates.yellow;
        }
        return false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player") || !canTeleport || !IsPlayerYellow(collider.gameObject))
        {
            return;
        }

        playerRb = collider.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            originalPlayerScale = collider.transform.localScale;
            StartCoroutine(CannonPortalSequence(collider.gameObject));
        }
    }

    private IEnumerator CannonPortalSequence(GameObject player)
    {
        if (!IsPlayerYellow(player)) yield break;

        canTeleport = false;
        playerRb.velocity = Vector2.zero;
        playerRb.isKinematic = true;

        StartCoroutine(ScalePlayerOverTime(player, Vector3.zero, 0.5f));
        yield return new WaitForSeconds(cannonHoldTime);

        if (!IsPlayerYellow(player))
        {
            playerRb.isKinematic = false;
            StartCoroutine(ScalePlayerOverTime(player, originalPlayerScale, 0.5f));
            yield break;
        }

        StartCoroutine(ScalePlayerOverTime(player, originalPlayerScale, 0.5f));
        playerRb.isKinematic = false;

        Vector2 normalizedDirection = transform.TransformDirection(ejectionDirection).normalized;
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

    public void SetEjectionDirection(Vector2 newDirection)
    {
        ejectionDirection = newDirection.normalized;
    }
}