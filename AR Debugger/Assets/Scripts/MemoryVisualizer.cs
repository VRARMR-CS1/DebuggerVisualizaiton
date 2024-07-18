using UnityEngine;
using System.Linq;
using Newtonsoft.Json.Linq;

public class MemoryVisualizer : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, -2f, 0); // Basic offset for positioning

    public void VisualizeAST(ASTNode astNode, Vector3 position)
    {
        UIManager uiManager = FindObjectOfType<UIManager>();

        switch (astNode.Type)
        {
            case "Module":
                Debug.Log("Visualizing AST Node: Module");
                foreach (ASTNode bodyNode in astNode.Body)
                {
                    VisualizeAST(bodyNode, position);
                    position += offset; // Move down for the next node
                }
                break;

            case "Assign":
                Debug.Log("Visualizing AST Node: Assign");
                foreach (ASTNode target in astNode.Targets)
                {
                    string valueText = GetValueText(astNode.Value);
                    string assignText = "Assign: " + target.Id + " = " + valueText;
                    Debug.Log(assignText);
                    uiManager.UpdateOutput(assignText);
                }
                position += offset; // Move down for the next node
                break;

            case "If":
                Debug.Log("Visualizing AST Node: If");
                string ifText = "If: " + astNode.Test.Left.Id + " > " + GetValueText(astNode.Test.Comparators[0]);
                Debug.Log(ifText);
                uiManager.UpdateOutput(ifText);
                position += offset; // Move down for the next node

                foreach (ASTNode bodyNode in astNode.Body)
                {
                    VisualizeAST(bodyNode, position);
                    position += offset; // Move down for the next node
                }
                break;

            case "Expr":
                Debug.Log("Visualizing AST Node: Expr");
                if (astNode.Value is ASTNode exprNode)
                {
                    VisualizeAST(exprNode, position); // Visualize the internal expression
                }
                break;

            case "Call":
                Debug.Log("Visualizing AST Node: Call");
                string argsText = string.Join(", ", astNode.Args.Select(a => GetValueText(a)).ToArray());
                string callText = "Call: " + astNode.Func.Id + "(" + argsText + ")";
                Debug.Log(callText);
                uiManager.UpdateOutput(callText);
                position += offset; // Move down for the next node
                break;

            case "BinOp":
                Debug.Log("Visualizing AST Node: BinOp");
                string leftText = GetValueText(astNode.Left);
                string rightText = GetValueText(astNode.Right);
                string opText = astNode.Op.Type;
                string binOpText = leftText + " " + opText + " " + rightText;
                Debug.Log(binOpText);
                uiManager.UpdateOutput(binOpText);
                position += offset; // Move down for the next node
                VisualizeAST(astNode.Left, position);
                position += offset;
                VisualizeAST(astNode.Right, position);
                break;

            case "Name":
                Debug.Log("Visualizing AST Node: Name");
                string nameText = astNode.Id;
                Debug.Log(nameText);
                uiManager.UpdateOutput(nameText);
                position += offset; // Move down for the next node
                break;

            case "Constant":
                Debug.Log("Visualizing AST Node: Constant");
                string constantText = GetValueText(astNode);
                Debug.Log(constantText);
                uiManager.UpdateOutput(constantText);
                position += offset; // Move down for the next node
                break;

            default:
                Debug.LogError("Unknown AST Node type: " + astNode.Type);
                break;
        }
    }

    private string GetValueText(object value)
    {
        if (value is ASTNode nodeValue)
        {
            return GetValueText(nodeValue.Value);
        }
        else if (value is JObject jObjectValue)
        {
            return jObjectValue["value"].ToString();
        }
        else
        {
            return value.ToString();
        }
    }
}
