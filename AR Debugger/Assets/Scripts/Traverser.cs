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
        // Visit each node of the AST
        foreach (ASTNode child in node.Body)
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
    }
}