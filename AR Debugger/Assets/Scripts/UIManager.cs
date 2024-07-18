using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public InputField inputField;
    public Button generateButton;
    public Text outputText;  // Output Text variable
    public ASTVisualizer astVisualizer;

    void Start()
    {
        generateButton.onClick.AddListener(OnGenerateButtonClick);
    }

    public void OnGenerateButtonClick()
    {
        outputText.text = ""; // Clear previous output
        string code = inputField.text.Replace("\\n", "\n");  // Replace \\n with \n
        astVisualizer.GetComponent<ASTFetcher>().StartFetch(code);
    }

    public void UpdateOutput(string output)
    {
        outputText.text += output + "\n"; // Append new output with a new line
        Debug.Log("Updated Output: " + outputText.text); // Debug the full output text
    }
}
