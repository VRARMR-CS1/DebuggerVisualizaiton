using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CodeStructure : MonoBehaviour
{
    public GameObject Headset;
    public Vector3 Location;
    private GameObject LineUp;

    void Start()
    {
        // Create a new GameObject a little in front and below the VR headset's location
        LineUp = new GameObject();
        LineUp.transform.position = Headset.transform.position + Headset.transform.forward * 0.5f - new Vector3(0, 0.6f, 0);
        Location = LineUp.transform.position;

    }

    public void VisualizeSequence(string codeLine, Vector3 location)
    {
        // Create a new GameObject for the line of code
        GameObject codeLineObject = new GameObject();
        codeLineObject.transform.position = location;

        // Add a TextMeshPro component to the GameObject and set its text to the code line
        TextMeshPro tmp = codeLineObject.AddComponent<TextMeshPro>();
        tmp.text = codeLine;

        // Force TextMeshPro to update so we can get the accurate bounds
        tmp.ForceMeshUpdate();

        // Get the bounds of the GameObject
        Bounds bounds = tmp.bounds;

        // Update the location for the next line of code
        // Move it to the right by the width of the current GameObject
        Location += new Vector3(bounds.size.x, 0, 0);
    }
}
