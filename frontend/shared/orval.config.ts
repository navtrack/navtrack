import { defineConfig } from "orval";

export default defineConfig({
  "navtrack-api": {
    output: {
      mode: "single",
      client: "react-query",
      // clean: true,
      prettier: true,
      target: "./src/api/index-generated.ts",
      schemas: "./src/api/model/generated",
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
});
