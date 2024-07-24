using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class ASTFetcher : MonoBehaviour
{
    public string pythonServerUrl = "http://127.0.0.1:5001/parse";
    public MemoryVisualizer visualizer;

    IEnumerator FetchAST(string code)
    {
        Debug.Log("Starting FetchAST...");
        var jsonData = new Dictionary<string, string>
        {
            { "code", code }
        };

        var jsonString = JsonConvert.SerializeObject(jsonData);
        var request = new UnityWebRequest(pythonServerUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        Debug.Log("Sending code to server: " + code);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Response received: " + request.downloadHandler.text);
            var jsonResponse = request.downloadHandler.text;
            var responseData = JsonConvert.DeserializeObject<JObject>(jsonResponse);
            var astJson = responseData["ast"].ToString();
            Debug.Log("Parsed AST JSON: " + astJson);

            ASTNode astNode = JsonConvert.DeserializeObject<ASTNode>(astJson);

            if (visualizer != null)
            {
                visualizer.VisualizeAST(astNode, Vector3.zero);
            }
            else
            {
                Debug.LogError("MemoryVisualizer is not assigned.");
            }
        }
    }

    public void StartFetch(string code)
    {
        Debug.Log("StartFetch method called with code: " + code);
        StartCoroutine(FetchAST(code));
    }
}
