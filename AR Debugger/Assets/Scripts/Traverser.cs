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
    private Dictionary<string, Vector3> lineupLocations = new Dictionary<string, Vector3>();

    public void Traverse(ASTNode node, List<string> pythonCode)
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

        // First phase: Create all lineup objects
        CreateLineupObjects(node, pythonCode);

        // Second phase: Update and visualize memory
        StartCoroutine(UpdateAndVisualizeMemory(node, pythonCode));
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
                    lineupLocations[code] = UserStructure.Location;
                    Debug.Log($"Recording location for {code}: {UserStructure.Location}");

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

    private IEnumerator UpdateAndVisualizeMemory(ASTNode node, List<string> pythonCode)
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

                    // Update the memory frame and visualize it
                    if (child.Type == "Assign" || child.Type == "AugAssign")
                    {
                        UserMemory.UpdateFrame(child);
                        if (lineupLocations.TryGetValue(code, out Vector3 location))
                        {
                            Debug.Log($"Visualizing memory at location for {code}: {location}");
                            UserMemory.Visualize(UserMemory.MemoryFrame, location);
                        }
                        else
                        {
                            Debug.LogError($"Location not found for code: {code}");
                        }
                    }

                    // Wait for 2 seconds before processing the next node
                    yield return new WaitForSeconds(2.0f);
                }
                else
                {
                    Debug.LogError("LineNumber is out of range: " + child.LineNumber);
                }

                // Recursively traverse the child nodes
                yield return StartCoroutine(UpdateAndVisualizeMemory(child, pythonCode));
            }
            else
            {
                Debug.LogError("child is null");
            }
        }
    }
}
