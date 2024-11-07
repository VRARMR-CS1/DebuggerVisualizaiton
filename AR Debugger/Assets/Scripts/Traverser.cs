using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using TMPro;
using System.Text;

public class Traverser : MonoBehaviour
{
    public CodeStructure UserStructure;
    public CodeMemory UserMemory;

    // Dictionary to store the locations of the lineups
    private Dictionary<int, Vector3> lineupLocations = new Dictionary<int, Vector3>(); // Changed to int key

    public void Traverse(ASTNode node, List<string> pythonCode, List<CodeFrame> frames)
    {
        if (node == null)
        {
            Debug.LogError("Node is null");
            return;
        }

        if (UserMemory == null)
        {
            Debug.LogError("UserMemory is null");
            return;
        }

        if (UserStructure == null)
        {
            Debug.LogError("UserStructure is null");
            return;
        }

        if (pythonCode == null)
        {
            Debug.LogError("PythonCode is null");
            return;
        }

        if (frames == null)
        {
            Debug.LogError("Frames list is null");
            return;
        }

        // First phase: Create all lineup objects
        CreateLineupObjects(node, pythonCode);

        // Second phase: Update and visualize memory
        StartCoroutine(UpdateAndVisualizeMemory(frames, pythonCode));
    }

    private void CreateLineupObjects(ASTNode node, List<string> pythonCode)
    {
        // Visit each node of the AST
        foreach (ASTNode child in node.Body ?? new List<ASTNode>())
        {
            if (child != null)
            {
                // Check if child is not null
                if (child.Type == "If" || child.Type == "For" || child.Type == "While" || child.Type == "IfExp" || child.Type == "With" || child.Type == "Try")
                {
                    continue;
                }

                // Get the corresponding string from the Python Code list
                if (child.LineNumber.HasValue && child.LineNumber.Value - 1 < pythonCode.Count)
                {
                    string code = pythonCode[child.LineNumber.Value - 1];

                    // Call the sequence function from the lineup class
                    UserStructure.VisualizeSequence(code, UserStructure.Location);

                    // Record the location of the lineup after creating it
                    lineupLocations[child.LineNumber.Value] = UserStructure.Location; // Use int key
                    Debug.Log($"Recording location for line {child.LineNumber.Value}: {UserStructure.Location}");

                    // Log the updated location after creating the lineup object
                    Debug.Log($"Updated location after creating {code}: {UserStructure.Location}");
                }
                else
                {
                    Debug.LogError("LineNumber is out of range: " + child.LineNumber);
                }

                // Recursively traverse the child nodes
                CreateLineupObjects(child, pythonCode);
            }
            else
            {
                Debug.LogError("child is null");
            }
        }
    }

    private IEnumerator UpdateAndVisualizeMemory(List<CodeFrame> frames, List<string> pythonCode)
    {
        foreach (var frame in frames)
        {
            int line = frame.Line;
            Debug.Log($"Processing frame for line: {line}");
            if (line - 1 < pythonCode.Count)
            {
                string code = pythonCode[line - 1];
                Debug.Log($"Animating memory for line: {line}");
                // Animate the memory frame first
                if (lineupLocations.TryGetValue(line, out Vector3 location))
                {
                    Debug.Log($"Visualizing memory at location for line {line}: {location}");
                    UserMemory.Visualize(UserMemory.MemoryFrame, location);
                }
                else
                {
                    Debug.LogError($"Location not found for line: {line}");
                }
                // Wait for animation to complete
                yield return new WaitForSeconds(2.0f);

                // Update the memory frame after the animation
                Debug.Log($"Updating memory for line: {line}");
                UserMemory.UpdateFrameFromJson(frame);
            }
            else
            {
                Debug.LogError("Line number is out of range: " + line);
            }
        }
    }


}
