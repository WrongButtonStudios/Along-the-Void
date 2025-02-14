using UnityEngine;

public class PlayerRedShrinkZone : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private characterController characterController;
    public float shrinkFactor = 0.1f;
    private Vector3 originalScale;

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
        if (collision.CompareTag("Player") && PlayerUttillitys.GetPlayerColor(characterController) == PlayerColor.red)
        {
            player.localScale = originalScale * shrinkFactor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.localScale = originalScale;
        }
    }
}