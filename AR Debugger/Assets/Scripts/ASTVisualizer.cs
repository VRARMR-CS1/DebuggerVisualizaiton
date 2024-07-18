using UnityEngine;

public class ASTVisualizer : MonoBehaviour
{
    public MemoryVisualizer memoryVisualizer;

    public void VisualizeAST(ASTNode astNode, Vector3 startPosition)
    {
        memoryVisualizer.VisualizeAST(astNode, startPosition);
    }
}
