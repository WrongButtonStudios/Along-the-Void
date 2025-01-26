using UnityEngine;
using System.Collections;

public class TeleportPortal : MonoBehaviour
{
    [SerializeField] private GameObject linkedPortal;
    [SerializeField] private float cooldown = 1f;
    private GameObject ps_YellowPortal_Startup_Spark;
    private GameObject ps_YellowPortal_FizzleOut;

    private bool canTeleport = true;
    private Rigidbody2D playerRb;
    [SerializeField] private characterController playerController;
    [SerializeField] private CharacterMovement characterMovement;
    private bool characterIsLookingRight;
    private Vector2 teleportDirection;


    void Awake()
    {
        ps_YellowPortal_Startup_Spark = transform.Find("PS_YellowPortal_Startup_Spark").gameObject;
        ps_YellowPortal_FizzleOut = transform.Find("PS_YellowPortal_FizzleOut").gameObject;
    }

    void Start()
    {
        if (linkedPortal == null)
        {
            Debug.LogError($"Teleport-Portal {gameObject.name} hat kein verbundenes Portal!");
            return;
        }

        TeleportPortal linkedScript = linkedPortal.GetComponent<TeleportPortal>();
        if (linkedScript != null && linkedScript.linkedPortal != gameObject)
        {
            Debug.LogWarning($"Portal {gameObject.name} hat eine einseitige Verbindung zu {linkedPortal.name}. Stelle bidirektionale Verbindung her...");
            linkedScript.linkedPortal = gameObject;
        }
    }

    void Update()
    {
        if (PlayerUttillitys.GetPlayerColor(playerController) == PlayerColor.yellow)
        {
            this.GetComponent<Collider2D>().enabled = true;
            ps_YellowPortal_Startup_Spark.SetActive(true);
            ps_YellowPortal_FizzleOut.SetActive(false);
        }
        else
        {
            this.GetComponent<Collider2D>().enabled = false;
            ps_YellowPortal_Startup_Spark.SetActive(false);
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
            TeleportToLinkedPortal();
        }
    }

    private void TeleportToLinkedPortal()
    {
        if (linkedPortal == null) return;

        TeleportPortal linkedPortalScript = linkedPortal.GetComponent<TeleportPortal>();
        if (linkedPortalScript == null) return;

        canTeleport = false;
        linkedPortalScript.canTeleport = false;

        characterIsLookingRight = characterMovement.GetCharacterLookingDirection();
        teleportDirection = (characterIsLookingRight ? linkedPortal.transform.right : -linkedPortal.transform.right)
            * playerRb.velocity.magnitude;

        playerRb.position = linkedPortal.transform.position;
        playerRb.velocity = teleportDirection;

        StartCoroutine(StartCooldown());
        StartCoroutine(linkedPortalScript.StartCooldown());
    }

    private IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        canTeleport = true;
    }

    public void LinkPortal(GameObject newPortal)
    {
        if (newPortal == null) return;

        TeleportPortal otherPortal = newPortal.GetComponent<TeleportPortal>();
        if (otherPortal != null)
        {
            linkedPortal = newPortal;
            otherPortal.linkedPortal = gameObject;
            Debug.Log($"Bidirektionale Verbindung hergestellt zwischen {gameObject.name} und {newPortal.name}");
        }
    }
}