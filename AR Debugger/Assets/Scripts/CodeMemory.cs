using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using TMPro;
using System.Text;

public class CodeMemory : MonoBehaviour
{
    public List<string> MemoryFrame = new List<string>();
    public TextMeshProUGUI MemoryFrameText;
    public GameObject Headset;
    public Dictionary<string, int> variables = new Dictionary<string, int>();
    public GameObject MovingObject;
    public GameObject movingObjectPrefab;

    public void Start()
    {
        Debug.Log("CodeMemory Start() called");

        // Instantiate the MovingObject from the prefab
        MovingObject = Instantiate(movingObjectPrefab);
        MovingObject.transform.SetParent(Headset.transform);
        MovingObject.transform.localPosition = new Vector3(6.2863f, -0.4f, 9.65f); // Set the local position directly
        MovingObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.6f); // Set the local scale as desired
        MovingObject.SetActive(true);
        Debug.Log($"Initial position of MovingObject: {MovingObject.transform.position}");

        // Create a Canvas as a child of the MovingObject
        GameObject canvasObject = new GameObject("Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvasObject.AddComponent<CanvasRenderer>();
        canvasObject.transform.SetParent(MovingObject.transform);

        // Position the canvas in front of the cube
        canvasObject.transform.localPosition = Vector3.zero;
        canvasObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        RectTransform rectTransform = canvasObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(13, 20);

        // Create a UI Text object and set its properties
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(canvasObject.transform);
        textObject.transform.localPosition = new Vector3(0, -6, -10); // Text position set to (0, -6, -10)
        textObject.transform.localScale = Vector3.one;

        MemoryFrameText = textObject.AddComponent<TextMeshProUGUI>();
        MemoryFrameText.text = "";
        MemoryFrameText.fontSize = 2; // Adjust font size for better visibility
        MemoryFrameText.color = Color.black; // Adjust text color for better visibility
        MemoryFrameText.rectTransform.sizeDelta = new Vector2(13, 20);

        Debug.Log("MemoryFrameText initialized");
    }

    public void UpdateFrameFromJson(CodeFrame frame)
    {
        MemoryFrame.Clear();
        // Directly access the frame's locals
        Dictionary<string, object> locals = frame.Locals;

        // Update MemoryFrame for locals
        foreach (var kvp in locals)
        {
            string varName = kvp.Key;
            string value = kvp.Value.ToString();
            Debug.Log($"Updating {varName} to {value} for line {frame.Line}");
            MemoryFrame.Add($"{varName} | {value}");
        }

            // Finally, update the display text
            StringBuilder sb = new StringBuilder();
        foreach (string line in MemoryFrame)
        {
            sb.AppendLine(line);
        }
        MemoryFrameText.text = sb.ToString();
    }


    public void Visualize(List<string> frame, Vector3 location)
    {
        Debug.Log("Visualize called");
        // Adjust the location to move the object a bit higher
        location.y += 1.3f; // Adjust the y-coordinate to move the object higher
        location.z += -0.3f;
        StartCoroutine(AnimateMemoryFrame(frame, location));
    }

    private IEnumerator AnimateMemoryFrame(List<string> frame, Vector3 location)
    {
        float duration = 2.0f; // Duration for each animation step
        float elapsedTime = 0f;

        Vector3 startPosition = MovingObject.transform.position;
        Vector3 endPosition = location;

        Debug.Log($"Animating from {startPosition} to {endPosition}");

        while (elapsedTime < duration)
        {
            MovingObject.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        MovingObject.transform.position = endPosition;

        StringBuilder sb = new StringBuilder();
        foreach (string line in frame)
            sb.AppendLine(line);
        MemoryFrameText.text = sb.ToString();
        Debug.Log("MemoryFrameText.text: " + MemoryFrameText.text);
    }
}