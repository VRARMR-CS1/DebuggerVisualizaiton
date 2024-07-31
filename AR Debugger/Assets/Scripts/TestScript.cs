using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Traverser traverser;

    void Start()
    {
        // Define a list of Python code strings
        List<string> pythonCode = new List<string>
        {
            "x = 1",
            "y = 2",
            "z = x + y",
        };

        // Define a JSON string that represents an AST
        string astJson = @"
{
    'type': 'Module',
    'body': [
        {
            'type': 'Assign',
            'targets': [{'type': 'Name', 'id': 'x'}],
            'value': {'type': 'Num', 'n': 1},
            'lineno': 1
        },
        {
            'type': 'Assign',
            'targets': [{'type': 'Name', 'id': 'y'}],
            'value': {'type': 'Num', 'n': 2},
            'lineno': 2
        },
        {
            'type': 'Assign',
            'targets': [{'type': 'Name', 'id': 'z'}],
            'value': {
                'type': 'BinOp',
                'left': {'type': 'Name', 'id': 'x'},
                'op': {'type': 'Add'},
                'right': {'type': 'Name', 'id': 'y'}
            },
            'lineno': 3
        }
    ]
}";


        // Convert the JSON string to an ASTNode object
        ASTNode ast = JsonConvert.DeserializeObject<ASTNode>(astJson);

        // Call the Traverse method with the Python code and AST
        traverser.Traverse(ast, pythonCode);
    }
}
