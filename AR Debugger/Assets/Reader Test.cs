using System.IO;
using UnityEngine;
using Python.Runtime;

public class MyScript : MonoBehaviour
{
    public GameObject myPrefab; // Assign your prefab in the inspector

    void Start()
    {
        PythonEngine.Initialize();
        using (Py.GIL()) // acquire the GIL
        {
            string path = Application.dataPath + "/PythonFolder/yourfile.py";

            // Read the content of the Python script file
            using (StreamReader sr = new StreamReader(path))
            {
                string pythonCode = sr.ReadToEnd();

                // Print the Python code to debug
                Debug.Log("Python Code: " + pythonCode);

                // Execute the Python code
                PythonEngine.RunSimpleString(pythonCode);
            }

            PyObject py = Py.Import("__main__");

            if (py.HasAttr("my_variable"))
            {
                int myVariable = py.GetAttr("my_variable").As<int>();

                if (myVariable > 4)
                {
                    // Instantiate your prefab if 'myVariable' is above 'someNumber'
                    GameObject myGameObject = Instantiate(myPrefab, Vector3.zero, Quaternion.identity);
                }
            }
        }    
    }
}
