using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineCamController : MonoBehaviour
{
    public float Direction = 0f;
    public float DirectionVertical = 0f;
    public float RightTrigger = 0f;
    public float RightStickButton = 0f;
    [SerializeField] public Rigidbody2D Fairy;
    public float Geschwindigkeit = 120f;
    public FairyHUDManager yellowFairyHUDManager;
    public FairyHUDManager blueFairyHUDManager;
    public FairyHUDManager redFairyHUDManager;
    public FairyHUDManager greenFairyHUDManager;
    public bool canMoveKeyboard = false;
    public bool canMoveController = false;
    public GameObject[] cameraList;
    private int currentCameraIndex;
    public PlayerController playerController;
    public DashScript dashScript;
    [SerializeField] private Transform speedVFX;
    Vector3 speedVFXSizeNormal;
    [SerializeField] Vector3 increasedSpeedVFX;


    void Start()
    {
        Debug.Log((int)RigidbodyConstraints2D.FreezeRotation + " " + (int)RigidbodyConstraints2D.FreezeAll); 
        Fairy = GetComponent<Rigidbody2D>();
        currentCameraIndex = 0;
        for (int i = 0; i < cameraList.Length; i++)
        {
        }

        if (cameraList.Length > 0)
        {
            cameraList[0].gameObject.SetActive(true);
        }
        speedVFXSizeNormal = speedVFX.localScale;

    }


    void Update()
    {
        if ((canMoveController || canMoveKeyboard) && playerController.isGrounded)
        {
            playerController.Player.velocity = new Vector2(playerController.Player.velocity.x * 0.0001f, playerController.Player.velocity.y * 0.0001f);
            playerController.Player.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            playerController.Player.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        Direction = Input.GetAxis("Fairy Horizontal");
        DirectionVertical = Input.GetAxis("Fairy Vertical");
        RightTrigger = Input.GetAxis("Right Trigger");

        if (Input.GetAxis("Right Trigger") > 0f || Input.GetKeyDown("g"))
        {
            yellowFairyHUDManager.enabled = true;
            blueFairyHUDManager.enabled = true;
            redFairyHUDManager.enabled = true;
            greenFairyHUDManager.enabled = true;
            canMoveKeyboard = false;
            canMoveController = false;
            playerController.enabled = true;
            dashScript.enabled = true;
            currentCameraIndex++;
            if (currentCameraIndex < cameraList.Length)
            {
                cameraList[currentCameraIndex - 1].gameObject.SetActive(false);
                cameraList[currentCameraIndex].gameObject.SetActive(true);
            }
        }
    }

    private void FixedUpdate()
    {
        MovementController();
        FarCam();
        Movement();
    }

    void Movement()
    {
        if (RecievedInput())
        {
            float velocityX = Direction * Geschwindigkeit;
            float velocityY = -DirectionVertical * Geschwindigkeit;

            if (Direction == 0f && DirectionVertical == 0f)
            {
                Fairy.velocity = Vector2.zero;
            }
            else
            {
                //To-Do: überprüfen ob bewegungen dadurch auf der Y Achse ruckartig werden
                Fairy.velocity = new Vector2(velocityX, velocityY);
            }

            if (Input.GetAxis("Left Trigger") > 0f && canMoveController && !canMoveKeyboard || Input.GetKey("q") && canMoveKeyboard && !canMoveController)
            {
                Debug.Log("Jawohl");
                speedVFX.localScale = speedVFXSizeNormal;
                currentCameraIndex = 3;
                SetNewCamera();
            }
        }

        if ((Input.GetAxis("Left Trigger") < 1f && canMoveController) && !canMoveKeyboard || !Input.GetKey("q") && canMoveKeyboard && !canMoveController)
        {
            Debug.Log("YES");
            speedVFX.localScale = speedVFXSizeNormal;
            currentCameraIndex = 0;
            SetNewCamera();
        }
    }

    void MovementController()
    {
        if (playerController.Geschwindigkeit > 20f && !canMoveController && playerController.Direction > 0f && !canMoveKeyboard)
        {
            speedVFX.localScale = speedVFXSizeNormal;
            currentCameraIndex = 2;
            SetNewCamera();
        }

        if (playerController.Geschwindigkeit > 20f && !canMoveController && playerController.Direction < 0f && !canMoveKeyboard)
        {
            speedVFX.localScale = speedVFXSizeNormal;
            currentCameraIndex = 5;
            SetNewCamera();
        }

        if (playerController.Geschwindigkeit < 20f && !canMoveController && !canMoveKeyboard && playerController.isGrounded)
        {
            speedVFX.localScale = speedVFXSizeNormal;
            currentCameraIndex = 1;
            SetNewCamera();
        }
    }

    void FarCam()
    {
        if (Input.GetAxis("Left Trigger") > 0f && !canMoveController && !canMoveKeyboard || Input.GetKey("q") && !canMoveController && !canMoveKeyboard || !playerController.isGrounded && playerController.Direction == 0f && dashScript.canDash == true)
        {
            speedVFX.localScale = increasedSpeedVFX;
            currentCameraIndex = 4;
            SetNewCamera();
        }
    }

    bool RecievedInput()
    {
        bool recievedInputController = Input.GetKeyDown(KeyCode.Joystick1Button9);
        bool recievedInputKeyboard = Input.GetKeyDown("f");
        if (recievedInputController || recievedInputKeyboard)
        {
            if (recievedInputController)
            {
                canMoveController = true;
                canMoveKeyboard = false;
            }
            else
            {
                canMoveKeyboard = true;
                canMoveController = false;
            }

            yellowFairyHUDManager.enabled = false;
            blueFairyHUDManager.enabled = false;
            redFairyHUDManager.enabled = false;
            greenFairyHUDManager.enabled = false;
            playerController.enabled = false;
            dashScript.enabled = false;
        }

        return recievedInputController || recievedInputKeyboard; 
    }

    void SetNewCamera()
    {
        foreach (GameObject cam in cameraList)
        {
            bool setActive = cameraList[currentCameraIndex] == cam; 
            cam.SetActive(setActive); 
        }
    }

}