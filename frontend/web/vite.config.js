import path from "path";
import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import dns from "dns";

dns.setDefaultResultOrder("verbatim");

// https://vitejs.dev/config/
export default defineConfig(() => {
  return {
    server: {
      host: "localhost",
      port: 3000,
      open: true
    },
    resolve: {
      alias: {
        "@navtrack/shared": path.resolve(__dirname, "../shared/src")
      }
    },
    build: {
      outDir: "build"
    },
    plugins: [react()]
  };
});
