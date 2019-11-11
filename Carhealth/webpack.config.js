
// Требуется для формирования полного output пути
const path = require('path');

module.exports = {
    mode: 'development', // development, production, none
     // Точка входа в приложение
    entry: './ClientVanilaJs/index.js',
     // Выходной файл
    output: {
        filename: 'bundle.js',
        path: path.resolve(__dirname, 'wwwroot/dist')
    },
    devtool: 'eval-source-map',
    // watch: true
};