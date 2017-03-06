var webpack = require("webpack");
var HtmlWebpackPlugin = require('html-webpack-plugin');

module.exports = {
    entry: {
        "vendor": "./Client/vendor.ts",
        "app": "./Client/app.ts"
    },
    output: {
        path: "./wwwroot",
        filename: "[name].bundle.js",
        publicPath: '/',
        chunkFilename: '[id].chunk.js'
    },
    resolve: {
        extensions: ['.ts', '.js']
    },
    devtool: 'source-map',
    module: {
        rules: [
            {
                test: /\.ts/,
                loaders: ['ts-loader', 'angular2-template-loader'],
                exclude: /node_modules/
            },
            {
                test: /\.html$/,
                loader: 'raw-loader'
            },
        ]
    },
    plugins: [
        new webpack.optimize.CommonsChunkPlugin({
            name: ['app', 'vendor']
        }),
        new HtmlWebpackPlugin({
            template: './Client/index.html',
            filename: 'index.html'
        }),
        new webpack.ProvidePlugin({
            jQuery: 'jquery', $: 'jquery', jquery: 'jquery'
        })
    ]
};