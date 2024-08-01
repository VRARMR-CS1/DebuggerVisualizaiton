import ast
import json
import sys
import os

class ASTTransformer(ast.NodeVisitor):
    def __init__(self):
        self.result = {}

    def visit(self, node):
        node_type = type(node).__name__
        if isinstance(node, ast.AST):
            node_fields = {}
            for field, value in ast.iter_fields(node):
                if isinstance(value, list):
                    node_fields[field] = [self.visit(v) if isinstance(v, ast.AST) else v for v in value]
                elif isinstance(value, ast.AST):
                    node_fields[field] = self.visit(value)
                else:
                    node_fields[field] = value
            return {node_type: node_fields}
        return node

    def transform(self, code):
        tree = ast.parse(code)
        transformed_ast = self.visit(tree)
        return transformed_ast

def process_code(code):
    try:
        transformed_ast = ASTTransformer().transform(code)
        return json.dumps({"type": "AST", "ast": transformed_ast}, indent=4)
    except SyntaxError as e:
        return json.dumps({"error": str(e)}, indent=4)

if __name__ == "__main__":
    code = sys.stdin.read()
    if not code:
        result = {"error": "Error"}
    else:
        result = process_code(code)
    
    file_path = os.path.join(os.path.dirname(__file__), 'data.json')
    with open(file_path, 'w') as file:
        file.write(result)
    
    print(result)
