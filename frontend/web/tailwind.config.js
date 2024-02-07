module.exports = {
  content: [
    "./src/**/*.{js,jsx,ts,tsx}",
    "./public/index.html",
    "../node_modules/flowbite-react/lib/esm/**/*.js"
  ],
  theme: {
    extend: {}
  },
  variants: {
    extend: {}
  },
  plugins: [require("@tailwindcss/forms"), require("flowbite/plugin")]
};
