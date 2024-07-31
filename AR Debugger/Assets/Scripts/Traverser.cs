using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using TMPro;
using System.Text;

public class Traverser : MonoBehaviour
{
    public CodeStructure UserStructure;
    public CodeMemory UserMemory;

    public void Traverse(ASTNode node, List<string> pythonCode)
    {
        if (node == null || node.Body == null)
        {
            Debug.LogError("Node or node.Body is null");
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
        // Visit each node of the AST
        foreach (ASTNode child in node.Body)
        {
            // Check if child is not null
            if (child != null)
            {
                // Empty code where the operations for conditionals & loops would be
                if (child.Type == "If" || child.Type == "For" || child.Type == "While" || child.Type == "IfExp" || child.Type == "With" || child.Type == "Try")
                {
                    continue;
                }

                // If it's an assign or augmented assign, add to or alter variable of memory frame values
                if (child.Type == "Assign" || child.Type == "AugAssign")
                {
                    UserMemory.UpdateFrame(child);
                }

                // Get the corresponding string from the Python Code list
                string code = pythonCode[child.LineNumber.Value - 1];

                // Call the sequence function from the lineup class
                UserStructure.VisualizeSequence(code, UserStructure.Location);

                // Call the memory frame sequence, passing the memory frame values and location for generation into it
                UserMemory.Visualize(UserMemory.MemoryFrame, UserStructure.Location);

                // Recursively traverse the child nodes
                Traverse(child, pythonCode);
            }
            else
            {
                Debug.Log("child is null");
            }
        }
    }

}