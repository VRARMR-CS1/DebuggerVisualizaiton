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
        LineUp.transform.position = Headset.transform.position + Headset.transform.forward * 0.5f - new Vector3(0, 0.6f, 0);
        Location = LineUp.transform.position;

    }

    public void VisualizeSequence(string codeLine, Vector3 location)
    {
        // Create a new GameObject for the line of code
        GameObject codeLineObject = Instantiate(lineUpPrefab, location, Quaternion.identity);

        // Add a TextMeshPro component to the GameObject and set its text to the code line
        TextMeshPro tmp = codeLineObject.AddComponent<TextMeshPro>();
        tmp.text = codeLine;

        // Force TextMeshPro to update so we can get the accurate bounds
        tmp.ForceMeshUpdate();

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
        // Move it to the right by the width of the current GameObject
        Location += new Vector3(lineUpBounds.size.x, 0, 0);
    }
}
