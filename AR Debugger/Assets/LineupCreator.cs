using System.IO;
using UnityEngine;

public class PythonReader : MonoBehaviour
{
    public GameObject sequentialPrefab;
    public GameObject decisionPrefab;
    public GameObject loopPrefab;

    private Vector3 nextPosition = Vector3.zero;

    void Start()
    {
        string path = "Assets/yourfile.py";
        string[] lines = File.ReadAllLines(path);

        foreach (string line in lines)
        {
            GameObject prefab = null;

            if (IsLoop(line))
            {
                prefab = loopPrefab;
            }
            else if (IsDecision(line))
            {
                prefab = decisionPrefab;
            }
            else if (IsSequential(line))
            {
                prefab = sequentialPrefab;
            }

            if (prefab != null)
            {
                Instantiate(prefab, nextPosition, Quaternion.identity);
                nextPosition.x += 1; // Adjust this value to control the distance between objects
            }
        }
    }

    bool IsSequential(string line)
    {
        // Add your logic here to determine if the line is a sequential statement
        return false;
    }

    bool IsDecision(string line)
    {
        // Decision statements in Python usually contain 'if' or 'else'
        return line.Contains("if") || line.Contains("else");
    }

    bool IsLoop(string line)
    {
        // Loop statements in Python usually contain 'for' or 'while'
        return line.Contains("for") || line.Contains("while");
    }
}
