using UnityEngine;
using TMPro; // TextMesh Pro 네임스페이스 추가

public class CodeStructure : MonoBehaviour
{
    public GameObject Headset;
    public Vector3 Location;
    public GameObject LineUp;
    public GameObject lineUpPrefab; // Unity 인스펙터에서 할당합니다.
    public float beltSpacing = 2.0f; // 098-belt(clone) 간격을 더 넓힘

    private int beltCounter = 0; // 몇 번째 벨트를 생성했는지 카운트

    void Start()
    {
        // VR 헤드셋의 위치에서 조금 앞과 아래에 새로운 GameObject를 생성합니다.
        GameObject beltObject = GameObject.Find("098-belt"); // 098-belt 오브젝트를 찾습니다.
        if (beltObject == null)
        {
            Debug.LogError("098-belt 오브젝트를 찾을 수 없습니다.");
            return;
        }

        LineUp = new GameObject();
        LineUp.transform.SetParent(beltObject.transform); // 부모를 098-belt로 설정합니다.
        LineUp.transform.position = Headset.transform.position + Headset.transform.forward * 0.1f - new Vector3(0, 0.6f, 0);
        Location = LineUp.transform.position;
    }

    public void VisualizeSequence(string codeLine, Vector3 location)
    {
        // 코드 라인을 위한 새로운 GameObject를 생성합니다.
        GameObject codeLineObject = Instantiate(lineUpPrefab, location + new Vector3(beltCounter * beltSpacing, 0, 0), lineUpPrefab.transform.rotation);
        codeLineObject.transform.SetParent(GameObject.Find("098-belt").transform); // 부모를 098-belt로 설정합니다.
        codeLineObject.SetActive(true);

        beltCounter++; // 벨트 카운터 증가

        // codeLineObject의 자식으로 Canvas를 생성합니다.
        GameObject canvasObject = new GameObject("Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvasObject.AddComponent<CanvasRenderer>();
        canvasObject.transform.SetParent(codeLineObject.transform);

        // 098-belt(Clone)의 Plane.004 앞에 Canvas를 위치시킵니다.
        Transform planeTransform = codeLineObject.transform.Find("Plane.004");
        if (planeTransform != null)
        {
            canvasObject.transform.position = planeTransform.position + new Vector3(0, 0.8f, -1f); // Plane.004 앞에 위치시키고, y축과 z축 위치 조정
        }
        else
        {
            Debug.LogError("Plane.004를 찾을 수 없습니다.");
        }

        canvasObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        RectTransform rectTransform = canvasObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(13, 20);

        // UI 텍스트 오브젝트를 생성하고 속성을 설정합니다.
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(canvasObject.transform);
        textObject.transform.localPosition = Vector3.zero;
        textObject.transform.localScale = Vector3.one;

        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
        text.text = codeLine;
        text.fontSize = 3; // 폰트를 더 작게 만듭니다.
        text.rectTransform.sizeDelta = new Vector2(13, 20);

        text.alignment = TextAlignmentOptions.Center;

        // 현재 오브젝트의 경계를 계산합니다.
        Bounds lineUpBounds;
        BoxCollider collider = codeLineObject.GetComponent<BoxCollider>();

        if (collider != null)
        {
            lineUpBounds = collider.bounds;
        }
        else
        {
            MeshRenderer renderer = codeLineObject.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                lineUpBounds = renderer.bounds;
            }
            else
            {
                Debug.LogError("LineUp 오브젝트에 BoxCollider나 MeshRenderer 컴포넌트가 없습니다.");
                return;
            }
        }

        // 현재 GameObject의 너비의 2배만큼 위치를 오른쪽으로 이동시킵니다.
        Location += new Vector3(lineUpBounds.size.x * 6, 0, 0);
    }
}
