const express = require('express');
const cors = require('cors');
const path = require('path');
const fs = require('fs');
const { spawn } = require('child_process');
const app = express();

app.use(cors());
app.use(express.json());
app.use(express.static(path.join(__dirname, 'public')));
app.use('/scripts', express.static(path.join(__dirname, 'scripts')));

app.post('/submit', (req, res) => {
    const code = req.body.code;

    const process = spawn('python', ['process_ast.py']);
    
    process.stdin.write(code);
    process.stdin.end();

    process.on('close', () => {
        fs.readFile(path.join(__dirname, 'data.json'), 'utf8', (error, data) => {
            if (error) return res.status(500).json({ error: 'Error' });
            res.json(JSON.parse(data));
        });
    });
});

app.post('/array', (req, res) => {
    const code = req.body.code;
    const result = { code: code.split('\n').map(line => line.trim()) };

    fs.writeFile(path.join(__dirname, 'dataArray.json'), JSON.stringify(result), error => {
        if (error) return res.status(500).json({ error: 'Error' });
        res.json(result);
    });
});

app.get('/data', (req, res) => {
    fs.readFile(path.join(__dirname, 'data.json'), 'utf8', (error, data) => {
        if (error) return res.status(500).json({ error: 'Error' });
        res.json(JSON.parse(data));
    });
});

app.get('/dataarray', (req, res) => {
    fs.readFile(path.join(__dirname, 'dataArray.json'), 'utf8', (error, data) => {
        if (error) return res.status(500).json({ error: 'Error' });
        res.json(JSON.parse(data));
    });
});

const PORT = 3001;
app.listen(PORT, () => {
    console.log(`localhost/${PORT}`);
});
