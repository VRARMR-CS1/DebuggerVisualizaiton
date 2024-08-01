using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CodeStructure : MonoBehaviour
{
    public GameObject Headset;
    public Vector3 Location;
    public GameObject LineUp;
    public GameObject lineUpPrefab; // Assign this in the Unity inspector

    void Start()
    {
        // Create a new GameObject a little in front and below the VR headset's location
        LineUp = new GameObject();
        LineUp.transform.position = Headset.transform.position + Headset.transform.forward * 0.1f - new Vector3(0, 0.6f, 0);
        Location = LineUp.transform.position;
    }

    public void VisualizeSequence(string codeLine, Vector3 location)
    {
        // Create a new GameObject for the line of code
        GameObject codeLineObject = Instantiate(lineUpPrefab, location, lineUpPrefab.transform.rotation);
        codeLineObject.SetActive(true);

        // Create a Canvas as a child of the codeLineObject
        GameObject canvasObject = new GameObject("Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvasObject.AddComponent<CanvasRenderer>();
        canvasObject.transform.SetParent(codeLineObject.transform);

        // Position the canvas in front of the higher part of the object
        canvasObject.transform.localPosition = new Vector3(1.5f, 1.5f, 2.1f); // Adjust the x position
        canvasObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        RectTransform rectTransform = canvasObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(13, 20);

        // Create a UI Text object and set its properties
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(canvasObject.transform);
        textObject.transform.localPosition = Vector3.zero;
        textObject.transform.localScale = Vector3.one;

        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
        text.text = codeLine;
        text.fontSize = 3; // Make the font smaller
        text.rectTransform.sizeDelta = new Vector2(13, 20);

        text.alignment = TextAlignmentOptions.Center;

        // Calculate the bounds of the current object
        Bounds lineUpBounds;
        BoxCollider collider = codeLineObject.GetComponent<BoxCollider>();

        if (collider != null)
        {
            lineUpBounds = collider.bounds;
        }
        else
        {
            MeshRenderer renderer = codeLineObject.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                lineUpBounds = renderer.bounds;
            }
            else
            {
                Debug.LogError("LineUp object does not have a BoxCollider or MeshRenderer component");
                return;
            }
        }

        // Move the location to the right by the width of the current GameObject
        Location += new Vector3(lineUpBounds.size.x, 0, 0);
    }
}
