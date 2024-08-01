document.addEventListener("DOMContentLoaded", function() {
    var editor = ace.edit("code-editor");
    editor.setTheme("ace/theme/monokai");
    editor.session.setMode("ace/mode/python");

    const form = document.getElementById('debugger-form');
    const outputDiv = document.getElementById('output');
    
    form.addEventListener('submit', async function submitFunction(event) {
        event.preventDefault();
        let code = editor.getValue();
        
        let response = await fetch('http://localhost:3001/submit', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ code: code })
        });
        let data = await response.json();
        console.log(data);
        outputDiv.textContent = JSON.stringify(data, null, 2);
    });


   
});
