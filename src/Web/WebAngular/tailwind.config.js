/** @type {import('tailwindcss').Config} */
module.exports = {
	darkMode: "class",
	content: [
		"./src/**/*.{html,ts}",
		"./src/index.html"
	],
	theme: {
		extend: {},
	},
	plugins: [
		require('prettier-plugin-tailwindcss'),
		require('@tailwindcss/forms'),
		require('@tailwindcss/typography'),
		require('flowbite/plugin')
	],
}

