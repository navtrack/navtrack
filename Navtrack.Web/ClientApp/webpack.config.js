/* webpack.config.js */
const { createWebpackDevConfig } = require('@craco/craco');
const cracoConfig = require('./craco.config.js');
const webpackConfig = createWebpackDevConfig(cracoConfig);

module.exports = webpackConfig;