/** @type {import('tailwindcss').Config} */
module.exports = {
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
		require('@tailwindcss/typography')
	],
}

