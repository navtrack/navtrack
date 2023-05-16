import path from "path";
import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vitejs.dev/config/
export default defineConfig(() => {
  return {
    define: {
      "process.env": {
        REACT_APP_API_URL: "http://localhost:3001"
      }
    },
    resolve: {
      alias: {
        "@navtrack/shared": path.resolve(__dirname, "../shared/src")
      }
    },
    build: {
      outDir: "build"
    },
    plugins: [react()],
    server: {}
  };
});
