using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FasterScroll : MonoBehaviour
{
    public Scrollbar scrollbar;             // Reference to the scrollbar
    public RectTransform content;          // The content container for dropdown items
    public float scrollSpeed = 20f;        // Speed multiplier for faster scrolling

    private void Update()
    {
        // Check for mouse scroll input
        float scrollInput = Input.mouseScrollDelta.y;

        if (scrollInput != 0 && content != null)
        {
            // Calculate the new vertical position of the content
            float newY = content.anchoredPosition.y + scrollInput * scrollSpeed;

            // Clamp the position to prevent scrolling beyond bounds
            newY = Mathf.Clamp(newY, 0, GetMaxScrollHeight());

            // Apply the new position to the content
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, newY);
        }
    }

    private float GetMaxScrollHeight()
    {
        // Calculate the maximum scrollable height based on content size and viewport size
        float contentHeight = content.rect.height;
        float viewportHeight = content.parent.GetComponent<RectTransform>().rect.height;
        return Mathf.Max(contentHeight - viewportHeight, 0);
    }
}