using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyHUDManager : MonoBehaviour
{
    public GameObject player;
    public float offset_x_rechts;
    public float offset_y_rechts;
    public float offset_x_links;
    public float offset_y_links;
    public float offsetSmoothing;
    private Vector3 playerPosition;
    public PlayerController playerController;

    void Update()
    {
        playerPosition = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);

        if (player.transform.localScale.x > 0f && playerController.isFacingRight)
        {
            playerPosition = new Vector3(playerPosition.x + offset_x_rechts, playerPosition.y + offset_y_rechts, playerPosition.z);
        }
        if (player.transform.localScale.y > 0f)
        {
            playerPosition = new Vector3(playerPosition.x + offset_x_rechts, playerPosition.y + offset_y_rechts, playerPosition.z);
        }

        if (player.transform.localScale.x < 0f && !playerController.isFacingRight)
        {
            playerPosition = new Vector3(playerPosition.x + -offset_x_links, playerPosition.y + offset_y_links, playerPosition.z);
        }
        if (player.transform.localScale.y < 0f)
        {
            playerPosition = new Vector3(playerPosition.x - offset_x_links, playerPosition.y + -offset_y_links, playerPosition.z);
        }
        //else
        //{
        //    playerPosition = new Vector3(playerPosition.x - offset_x_links, playerPosition.y + offset_y_links, playerPosition.z);
        //}
        //else
        //{
        //    playerPosition = new Vector3(playerPosition.x - offset_x_links, playerPosition.y - offset_y_links, playerPosition.z);
        //}

        transform.position = Vector3.Lerp(transform.position, playerPosition, offsetSmoothing * Time.deltaTime);
    }
}