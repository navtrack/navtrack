import { defineConfig } from "orval";

export default defineConfig({
  "navtrack-api": {
    output: {
      mode: "single",
      client: "react-query",
      clean: true,
      prettier: true,
      target: "./src/api/index.ts",
      schemas: "./src/api/model",
      override: {
        mutator: {
          path: "./src/axios/authAxiosInstance.ts",
          name: "authAxiosInstance"
        }
      }
    },
    input: {
      target: "../../api.json"
    }
  }
});
