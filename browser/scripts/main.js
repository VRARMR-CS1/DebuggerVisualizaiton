document.addEventListener("DOMContentLoaded", function() {
    var editor = ace.edit("code-editor");
    editor.setTheme("ace/theme/monokai");
    editor.session.setMode("ace/mode/python");

    const form = document.getElementById('debugger-form');
    form.addEventListener('submit', submitFunction);

    function submitFunction(event) {
        event.preventDefault();
        let code = editor.getValue();
        fetch('http://127.0.0.1:3000/submit', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ code: code })
        })
        .then(response => {
            if (!response.ok) {
                throw new Error(`${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            console.log(data);
        })
        .catch((error) => {
            console.error(error);
        });
    }
});
