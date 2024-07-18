from flask import Flask, request, jsonify
import ast
import json

app = Flask(__name__)

@app.route('/parse', methods=['POST'])
def parse_code():
    try:
        data = request.json
        code = data['code']
        app.logger.debug(f"Received code: {code}")
        tree = ast.parse(code)
        ast_json = ast_to_json(tree)
        return jsonify({'ast': ast_json})
    except Exception as e:
        app.logger.error(f"Error parsing code: {str(e)}")
        return jsonify({'error': str(e)}), 500

def ast_to_json(node):
    if isinstance(node, ast.AST):
        fields = {a: ast_to_json(b) for a, b in ast.iter_fields(node)}
        return {'_type': node.__class__.__name__, **fields}
    elif isinstance(node, list):
        return [ast_to_json(x) for x in node]
    else:
        return node

if __name__ == '__main__':
    app.run(port=5001, debug=True)
