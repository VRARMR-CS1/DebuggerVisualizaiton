using UnityEngine;

public class SetCameraPosition : MonoBehaviour
{
    public Vector3 newPosition = new Vector3(10f, 3f, -20f);

    void Start()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.transform.position = newPosition;
            Debug.Log("Camera position set to: " + mainCamera.transform.position);
        }
        else
        {
            Debug.LogError("MainCamera not found! Make sure your camera is tagged as 'MainCamera'.");
        }
    }
}
