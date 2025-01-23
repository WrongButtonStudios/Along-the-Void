using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class fairy : MonoBehaviour
{
    public enum fairyColor
    {
        green,
        red,
        blue,
        yellow
    }

    [System.Serializable]
    public class fairyData
    {
        public fairyColor color;
        [Range(0,1)] public float colorAmount;
        public GameObject fairyObject;
    }

    public GameObject shape;
    public SpriteRenderer spriteRenderer;
    public SpriteMask spriteMask;

    public bool empty = false;

    private fairyController fairyController;
    private float colorAmount;

    [SerializeField] private int fairyDataIndex = -1;
    [Space]
    [SerializeField] private Color greenColor;
    [SerializeField] private Color redColor;
    [SerializeField] private Color blueColor;
    [SerializeField] private Color yellowColor;

    private void Start() {
        fairyController = GetComponentInParent<fairyController>();
    }

    private void Update() {
        if(fairyDataIndex != -1) {
            if(fairyController.fairys[fairyDataIndex].colorAmount != colorAmount) {
                colorAmount = fairyController.fairys[fairyDataIndex].colorAmount;

                Vector3 newMaskPosition = spriteMask.transform.localPosition;
                newMaskPosition.y = Mathf.Lerp(-spriteMask.transform.localScale.x,0f,colorAmount);
                spriteMask.transform.localPosition = newMaskPosition;
            }
        }
        if(colorAmount <= 0.0f && !empty) {
            fairyController.SwitchColorBecauseOfEmptyColorThatsWhy();
            empty = true;
        }
    }

    public void setIndex(int index) {
        fairyDataIndex = index;

        spriteRenderer.sortingLayerName = "fairyMask" + index;
        spriteMask.frontSortingLayerID = SortingLayer.NameToID("fairyMask" + index);
        spriteMask.backSortingLayerID = SortingLayer.NameToID("fairyMask" + (index - 1));
    }

    public void setColor(fairyColor colorToSet) {
        switch(colorToSet) {
            case fairyColor.green:
                spriteRenderer.color = greenColor;
                break;

            case fairyColor.red:
                spriteRenderer.color = redColor;
                break;

            case fairyColor.blue:
                spriteRenderer.color = blueColor;
                break;

            case fairyColor.yellow:
                spriteRenderer.color = yellowColor;
                break;
        }
    }
}
