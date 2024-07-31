using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Traverser traverser;

    void Start()
    {
        Debug.Log("TestScript Start() called");

        // Define a list of Python code strings
        List<string> pythonCode = new List<string>
        {
            "a = 1",
            "b = 2",
            "c = a + b"
        };

        // Define a JSON string that represents an AST
        string astJson = @"
{
    'type': 'Module',
    'body': [
        {
            'type': 'Assign',
            'targets': [{'type': 'Name', 'id': 'a'}],
            'value': {'type': 'Num', 'n': 1},
            'lineno': 1
        },
        {
            'type': 'Assign',
            'targets': [{'type': 'Name', 'id': 'b'}],
            'value': {'type': 'Num', 'n': 2},
            'lineno': 2
        },
        {
            'type': 'Assign',
            'targets': [{'type': 'Name', 'id': 'c'}],
            'value': {
                'type': 'BinOp',
                'left': {'type': 'Name', 'id': 'a'},
                'op': {'type': 'Add'},
                'right': {'type': 'Name', 'id': 'b'}
            },
            'lineno': 3
        }
    ]
}";

        // Convert the JSON string to an ASTNode object
        ASTNode ast;
        try
        {
            ast = JsonConvert.DeserializeObject<ASTNode>(astJson);
            if (ast == null)
            {
                Debug.LogError("Failed to deserialize AST.");
                return;
            }
        }
        catch (JsonException e)
        {
            Debug.LogError("JSON deserialization error: " + e.Message);
            return;
        }

        // Ensure traverser is not null
        if (traverser == null)
        {
            Debug.LogError("Traverser is not assigned.");
            return;
        }

        Debug.Log("Starting traversal");

        // Call the Traverse method with the Python code and AST
        try
        {
            traverser.Traverse(ast, pythonCode);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error during traversal: " + e.Message);
        }

        Debug.Log("Traversal complete");
    }
}
