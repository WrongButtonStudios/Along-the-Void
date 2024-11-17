using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class fairyController : MonoBehaviour
{
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

    private Vector2 selectInput;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if(lastFairyCount != fairys.Count || copyOfFairyRadius != fairyRadius)
        {
            spawnFairys();
        }

        selectFairy();

        if(selectedFairy != lastSelectedFairy)
        {
            debugSetPlayerState();
        }

        lastSelectedFairy = selectedFairy;
        lastSelectMagnitude = selectInput.magnitude;
        lastFairyCount = fairys.Count;
        copyOfFairyRadius = fairyRadius;
    }

    //for debugging and testing. this isnt how its supposed to be done
    private void debugSetPlayerState()
    {
        characterController player = FindObjectOfType<characterController>();

        switch (fairys[selectedFairy].color)
        {
            case fairy.fairyColor.green:
                player.transitionToState(characterController.playerStates.green);
                break;

            case fairy.fairyColor.red:
                player.transitionToState(characterController.playerStates.red);
                break;

            case fairy.fairyColor.blue:
                player.transitionToState(characterController.playerStates.blue);

                //Debug.LogWarning("im going to implement this very soon");
                break;

            case fairy.fairyColor.yellow:
                //player.transitionToState(characterController.playerStates.yellow, true);

                Debug.LogWarning("im going to implement this very soon");
                break;
        }
    }

    private void selectFairy()
    {
        if (selectInput.magnitude != 0)
        {
            Vector3 lineTragetPos = selectInput * fairyRadius;

            lineRenderer.SetPosition(1, lineTragetPos);

            if (selectInput.magnitude > 0.1f)
            {
                selectPos = lineTragetPos;
            }
        }
        else
        {
            lineRenderer.SetPosition(1, Vector3.zero);
        }

        if (selectInput.magnitude > Mathf.Epsilon)
        {
            int closestFairyIndex = -1;
            float closestDistance = Mathf.Infinity;

            for (int i = 0; i < fairys.Count; i++)
            {
                float distance = Vector3.Distance(fairys[i].fairyObject.transform.localPosition, selectPos);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestFairyIndex = i;
                }
            }

            if (closestFairyIndex != -1)
            {
                selectedFairy = closestFairyIndex;
            }
        }
    }

    [ContextMenu("respawn fairys")]
    private void spawnFairys()
    {
        deleteFairys();

        for(int i = 0; i < fairys.Count; i++)
        {
            float angle = i * Mathf.PI * 2 / fairys.Count;
            Vector3 fairyPosition = new Vector2(Mathf.Cos(angle) * fairyRadius, Mathf.Sin(angle) * fairyRadius);
            fairyPosition += transform.position;

            GameObject newFairyObject = Instantiate(fairyPrefab, fairyPosition, Quaternion.identity, this.transform);
            fairy newFairy = newFairyObject.GetComponent<fairy>();

            newFairy.setColor(fairys[i].color);
            newFairy.setIndex(i);

            fairy.fairyData newFairyData = fairys[i];
            newFairyData.fairyObject = newFairyObject;
            fairys[i] = newFairyData;
        }
    }

    private void deleteFairys()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void getSelectInput(InputAction.CallbackContext context)
    {
        selectInput = context.ReadValue<Vector2>();
    }
}
