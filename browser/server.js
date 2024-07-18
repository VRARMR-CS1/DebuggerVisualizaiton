const express = require('express');
const cors = require('cors'); 
const path = require('path');
const app = express();


app.use(cors());

app.use(express.json());

app.use(express.static(path.join(__dirname, 'public')));

app.use('/scripts', express.static(path.join(__dirname, 'scripts')));

app.post('/submit', (req, res) => {
    const code = req.body.code;
    console.log(code);

    res.json({ message: 'Success', code: code });
});

const PORT = 3000;
app.listen(PORT, () => {
    console.log(`http://localhost:${PORT}`);
});
