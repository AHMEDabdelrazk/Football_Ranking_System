/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  darkMode: 'class',
  theme: {
    extend: {
      colors: {
        pitch: {
          dark: '#0a0e17',
          surface: '#121826',
          card: '#1a2234',
          border: '#26334d',
          accent: '#10b981',
          gold: '#f59e0b',
          blue: '#3b82f6',
          purple: '#8b5cf6'
        }
      }
    },
  },
  plugins: [],
}
