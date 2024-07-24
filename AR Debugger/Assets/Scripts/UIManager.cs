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
        outputText.text = ""; // 이전 출력 지우기
        string code = inputField.text;  // \\n을 \n으로 대체할 필요 없음, 그대로 가져오기
        astVisualizer.GetComponent<ASTFetcher>().StartFetch(code);
    }

    public void UpdateOutput(string output)
    {
        outputText.text += output + "\n"; // Append new output with a new line
        Debug.Log("Updated Output: " + outputText.text); // Debug the full output text
    }
}
