using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static characterController;
using static fairy;

public class fairyController : MonoBehaviour
{
    public static fairyController Instance;
    public GameObject fairyPrefab;
    [Space]
    public List<fairy.fairyData> fairys = new List<fairy.fairyData>();
    [SerializeField] private int selectedFairy;
    [Space]
    [SerializeField] private float fairyRadius = 0.5f;

    private int lastFairyCount;
    private float copyOfFairyRadius;
    private Vector3 selectPos;
    private float lastSelectMagnitude;
    private int lastSelectedFairy;
    private bool _input = false;
    private Vector2 selectInput;

    public int emptycolors = 0;

    private LineRenderer lineRenderer;

    private void Awake() {
        Instance = this;
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update() {
        if(lastFairyCount != fairys.Count || copyOfFairyRadius != fairyRadius) {
            spawnFairys();
        }

        selectFairy();

        if(_input) {
            _input = false;
            debugSetPlayerState();
        }

        lastSelectedFairy = selectedFairy;
        lastSelectMagnitude = selectInput.magnitude;
        lastFairyCount = fairys.Count;
        copyOfFairyRadius = fairyRadius;

        //if(fairys[selectedFairy].colorAmount <= 0.0f) {
        //}

    }

    public void SwitchColorBecauseOfEmptyColorThatsWhy() {
        characterController player = FindObjectOfType<characterController>();
        switch(player.StatusData.currentState) {
            case characterController.playerStates.red:
                player.transitionToState(characterController.playerStates.yellow);
                break;
            case characterController.playerStates.burntRed:
                player.transitionToState(characterController.playerStates.yellow);
                break;
            case characterController.playerStates.yellow:
                player.transitionToState(characterController.playerStates.blue);
                break;
            case characterController.playerStates.burntYellow:
                player.transitionToState(characterController.playerStates.blue);
                break;
            case characterController.playerStates.blue:
                player.transitionToState(characterController.playerStates.green);
                break;
            case characterController.playerStates.burntBlue:
                player.transitionToState(characterController.playerStates.green);
                break;
            case characterController.playerStates.green:
                player.transitionToState(characterController.playerStates.red);
                break;
            case characterController.playerStates.burntGreen:
                player.transitionToState(characterController.playerStates.red);
                break;
        }
    }

    //for debugging and testing. this isnt how its supposed to be done
    public void debugSetPlayerState() {
        characterController player = FindObjectOfType<characterController>();

        switch(fairys[selectedFairy].color) {
            case fairy.fairyColor.green:
                player.transitionToState(characterController.playerStates.green);
                break;

            case fairy.fairyColor.red:
                player.transitionToState(characterController.playerStates.red);
                break;

            case fairy.fairyColor.blue:
                player.transitionToState(characterController.playerStates.blue);
                break;

            case fairy.fairyColor.yellow:
                player.transitionToState(characterController.playerStates.yellow);
                break;
        }
    }

    private void selectFairy() {
        if(selectInput.magnitude != 0) {
            Vector3 lineTragetPos = selectInput * fairyRadius;

            lineRenderer.SetPosition(1,lineTragetPos);

            if(selectInput.magnitude > 0.1f) {
                selectPos = lineTragetPos;
            }
        }
        else {
            lineRenderer.SetPosition(1,Vector3.zero);
        }

        if(selectInput.magnitude > Mathf.Epsilon) {
            int closestFairyIndex = -1;
            float closestDistance = Mathf.Infinity;

            for(int i = 0; i < fairys.Count; i++) {
                float distance = Vector3.Distance(fairys[i].fairyObject.transform.localPosition,selectPos);

                if(distance < closestDistance) {
                    closestDistance = distance;
                    closestFairyIndex = i;
                }
            }

            if(closestFairyIndex != -1) {
                _input = true;
                selectedFairy = closestFairyIndex;
            }
        }
    }

    [ContextMenu("respawn fairys")]
    public void spawnFairys() {
        deleteFairys();

        for(int i = 0; i < fairys.Count; i++) {
            float angle = i * Mathf.PI * 2 / fairys.Count;
            Vector3 fairyPosition = new Vector2(Mathf.Cos(angle) * fairyRadius,Mathf.Sin(angle) * fairyRadius);
            fairyPosition += transform.position;

            GameObject newFairyObject = Instantiate(fairyPrefab,fairyPosition,Quaternion.identity,this.transform);
            fairy newFairy = newFairyObject.GetComponent<fairy>();

            newFairy.setColor(fairys[i].color);
            newFairy.setIndex(i);

            fairy.fairyData newFairyData = fairys[i];
            newFairyData.fairyObject = newFairyObject;
            fairys[i] = newFairyData;
            fairys[i].colorAmount = 1.0f;
        }
    }

    private void deleteFairys() {
        foreach(Transform child in transform) {
            Destroy(child.gameObject);
        }
    }

    public void getSelectInput(InputAction.CallbackContext context) {
        selectInput = context.ReadValue<Vector2>();
    }
}