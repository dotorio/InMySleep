import { fileURLToPath, URL } from "node:url";

import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
// import fs from "fs";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue()],
  // server: {
  //   https: {
  //     key: fs.readFileSync("src/privkey1.pem"),
  //     cert: fs.readFileSync("src/cert1.pem"),
  //     ca: fs.readFileSync("src/chain1.pem"),
  //   },
  // },
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    },
  },
});
