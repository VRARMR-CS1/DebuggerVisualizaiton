using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using TMPro;
using System.Text;

public class CodeMemory : MonoBehaviour
{
    public List<string> MemoryFrame = new List<string>();
    public TextMeshPro MemoryFrameText;
    public GameObject Headset;
    public Dictionary<string, int> variables = new Dictionary<string, int>();
    public GameObject MovingObject;

    public void Start()
    {
        MovingObject = new GameObject();
        MovingObject.transform.position = Headset.transform.position + Headset.transform.forward * 0.5f - new Vector3(0, 0.5f, 0);

        // Add a TextMeshPro component to the GameObject and set its text to empty
        MemoryFrameText = MovingObject.AddComponent<TextMeshPro>();
        MemoryFrameText.text = "";

    }

    public void UpdateFrame(ASTNode node)
    {
        string varName = null;
        string value = null;

        if (node.Targets != null)
        {
            var target = node.Targets[0];
            var idProperty = target.GetType().GetProperty("Id");
            if (idProperty != null)
            {
                varName = idProperty.GetValue(target).ToString();

                if (node.Value is JObject)
                {
                    JObject jObject = node.Value as JObject;
                    Debug.Log("node.Value: " + node.Value);

                    if (jObject == null)
                    {
                        Debug.LogError("node.Value cannot be cast to JObject");
                        return;
                    }

                    JToken typeToken;
                    if (!jObject.TryGetValue("Type", out typeToken))
                    {
                        Debug.LogError("JObject does not contain a 'Type' property");
                        return;
                    }

                    var valueType = typeToken.ToString();
                    if (valueType == "Num")
                    {
                        value = jObject["N"].ToString();
                        // Update the value of the variable in the dictionary
                        variables[varName] = int.Parse(value);
                    }
                    else if (valueType == "BinOp")
                    {
                        var left = jObject["Left"]["Id"].ToString();
                        var op = jObject["Op"]["Type"].ToString();
                        var right = jObject["Right"]["N"].ToString();

                        // Evaluate the operation using the values of the operands from the dictionary
                        int result = 0;
                        if (op == "Add")
                        {
                            result = variables[left] + int.Parse(right);
                        }
                        else if (op == "Sub")
                        {
                            result = variables[left] - int.Parse(right);
                        }
                        else if (op == "Mult")
                        {
                            result = variables[left] * int.Parse(right);
                        }
                        else if (op == "Div")
                        {
                            result = variables[left] / int.Parse(right);
                        }
                        else if (op == "Mod")
                        {
                            result = variables[left] % int.Parse(right);
                        }
                        // Add other operations as needed.

                        value = result.ToString();
                        // Update the value of the variable in the dictionary
                        variables[varName] = result;
                    }
                }
            }
        }

        if (varName != null && value != null)
        {
            // Check if variable already exists in the memory frame
            for (int i = 0; i < MemoryFrame.Count; i++)
            {
                if (MemoryFrame[i].StartsWith(varName + " ="))
                {
                    // Variable found, update its value
                    MemoryFrame[i] = $"{varName} | {value}";
                    return;
                }
            }

            // Variable not found, add it to the memory frame
            MemoryFrame.Add($"{varName} | {value}");
        }
    }

    public void Visualize(List<string> frame, Vector3 location)
    {
        // Move the GameObject smoothly from its current location to the new location
        MovingObject.transform.position = Vector3.Lerp(MovingObject.transform.position, location, Time.deltaTime);

        // Update the text of the GameObject with the contents of the list
        StringBuilder sb = new StringBuilder();
        foreach (string line in frame)
        {
            sb.AppendLine(line);
        }
        MemoryFrameText.text = sb.ToString();
    }
}