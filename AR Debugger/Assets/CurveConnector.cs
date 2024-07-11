using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] 
public class CurveConnector : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public int numberOfPoints = 20;
    public float lineWidth = 0.1f; 
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth; 
        lineRenderer.endWidth = lineWidth; 
        DrawQuadraticCurve();
    }

    void Update() 
    {
        if (startPoint != null && endPoint != null)
        {
            lineRenderer.startWidth = lineWidth; 
            lineRenderer.endWidth = lineWidth; 
            DrawQuadraticCurve();
        }
    }

    void DrawQuadraticCurve()
    {
        Vector3 p0 = startPoint.position + new Vector3(startPoint.localScale.x / 2, 0, 0); 
        Vector3 p2 = endPoint.position + new Vector3(endPoint.localScale.x / 2, 0, 0); 
        Vector3 p1 = (p0 + p2) / 2 + Vector3.up * 2; 

        lineRenderer.positionCount = numberOfPoints;
        for (int i = 0; i < numberOfPoints; i++)
        {
            float t = i / (float)(numberOfPoints - 1);
            Vector3 position = CalculateQuadraticBezierPoint(t, p0, p1, p2);
            lineRenderer.SetPosition(i, position);
        }
    }

    Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0; // (1-t)^2 * p0
        p += 2 * u * t * p1; // 2(1-t)t * p1
        p += tt * p2; // t^2 * p2
        return p;
    }
}