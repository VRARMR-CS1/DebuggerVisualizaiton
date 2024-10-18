using UnityEngine;
using TMPro; // Add TextMesh Pro namespace

public class CodeStructure : MonoBehaviour
{
    public GameObject Headset;
    public Vector3 Location;
    public GameObject LineUp;
    public GameObject lineUpPrefab; // Assign in the Unity inspector.
    public float beltSpacing = 1.0f; // Increase the spacing between 098-belt(clone)

    private int beltCounter = 0; // Counter to track the number of belts created

    void Start()
    {
        // Create a new GameObject slightly in front and below the VR headset's location.
        GameObject beltObject = GameObject.Find("098-belt"); // Find the 098-belt object.
        if (beltObject == null)
        {
            Debug.LogError("Cannot find the 098-belt object.");
            return;
        }

        // Place the beltObject in front of the headset and adjust its size and position
        beltObject.transform.position = Headset.transform.position + Headset.transform.forward * 0.7f - new Vector3(0, 0.5f, 0);
        beltObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // Adjust the scale to make it smaller
        beltObject.transform.SetParent(Headset.transform);

        LineUp = new GameObject();
        LineUp.transform.SetParent(beltObject.transform); // Set the parent to 098-belt.

        // Set the local position relative to the beltObject
        LineUp.transform.localPosition = new Vector3(0, -0.6f, 0.1f); // Adjust the values as needed
        Location = LineUp.transform.position;

        Debug.Log($"Headset position: {Headset.transform.position}");
        Debug.Log($"beltObject position: {beltObject.transform.position}");
        Debug.Log($"LineUp local position: {LineUp.transform.localPosition}");
        Debug.Log($"LineUp world position: {LineUp.transform.position}");
    }

    public void VisualizeSequence(string codeLine, Vector3 location)
    {
        // Create a new GameObject for the code line.
        GameObject codeLineObject = Instantiate(lineUpPrefab, location + new Vector3(beltCounter * beltSpacing, 0, 0), lineUpPrefab.transform.rotation);
        codeLineObject.transform.SetParent(GameObject.Find("098-belt").transform); // Set the parent to 098-belt.
        codeLineObject.SetActive(true);

        beltCounter++; // Increase the belt counter

        // Create a Canvas as a child of the codeLineObject.
        GameObject canvasObject = new GameObject("Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvasObject.AddComponent<CanvasRenderer>();
        canvasObject.transform.SetParent(codeLineObject.transform);

        // Position the Canvas in front of Plane.004 of 098-belt(Clone).
        Transform planeTransform = codeLineObject.transform.Find("Plane.004");
        if (planeTransform != null)
        {
            canvasObject.transform.position = planeTransform.position + new Vector3(0, 0.8f, -1f); // Position in front of Plane.004, adjusting the y and z axes
        }
        else
        {
            Debug.LogError("Cannot find Plane.004.");
        }

        canvasObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        RectTransform rectTransform = canvasObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(13, 20);

        // Create a UI text object and set its properties.
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(canvasObject.transform);
        textObject.transform.localPosition = Vector3.zero;
        textObject.transform.localScale = Vector3.one;

        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
        text.text = codeLine;
        text.fontSize = 3; // Make the font smaller
        text.rectTransform.sizeDelta = new Vector2(13, 20);

        text.alignment = TextAlignmentOptions.Center;

        // Calculate the bounds of the current object.
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
                Debug.LogError("LineUp object does not have a BoxCollider or MeshRenderer component.");
                return;
            }
        }

        // Move the location to the right by twice the width of the current GameObject.
        Location += new Vector3(lineUpBounds.size.x * 5, 0, 0);
    }
}
