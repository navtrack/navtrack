const path = require("path");
const { override, addPostcssPlugins } = require("customize-cra");

const sharedRepoPath = path.resolve("../shared/src");

function myOverrides(config, env) {
  // Remove ModuleScopePlugin
  config.resolve.plugins = config.resolve.plugins.filter((plugin) => {
    return plugin.constructor.name !== "ModuleScopePlugin";
  });

  // Enable compiling outside of src/
  config.module.rules[1].oneOf = config.module.rules[1].oneOf.map((rule) => {
    if (rule.loader && rule.loader.match(/\/babel-loader\//) && rule.include) {
      return {
        ...rule,
        include: [rule.include, sharedRepoPath]
      };
    }

    return rule;
  });

  // Mirror TypeScript @shared path in webpack
  config.resolve.alias["@navtrack/ui-shared"] = sharedRepoPath;

  return config;
}

module.exports = override(
  myOverrides,
  addPostcssPlugins([require("tailwindcss"), require("autoprefixer")])
);
