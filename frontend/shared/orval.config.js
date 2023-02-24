module.exports = {
  "navtrack-api": {
    output: {
      mode: "single",
      target: "./src/api/index-generated.ts",
      schemas: "./src/api/model/generated",
      client: "react-query",
      override: {
        mutator: {
          path: "./src/api/authAxiosInstance.ts",
          name: "authAxiosInstance"
        }
      }
    },
    input: {
      target: "../../api.json"
    }
  }
};
