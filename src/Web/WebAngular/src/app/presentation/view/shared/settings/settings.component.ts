import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
	public isSettingsOpen: boolean = false;
	public theme: string = "light";

	ngOnInit(): void {
		this.initTheme();
	}

	toggleSettings(): void {
		this.isSettingsOpen = !this.isSettingsOpen;
	}

	private initTheme(): void {
		const darkThemeSettings: string | null = localStorage.getItem("theme");

		if (darkThemeSettings == null) {
			if (window.matchMedia('(prefers-color-scheme: dark)').matches) {
				localStorage.setItem("theme", "dark");
			} else {
				localStorage.setItem("theme", "light");
			}
		}

		if (darkThemeSettings == "dark") {
			document.querySelector("html")?.classList.add("dark");
			this.theme = "dark";
		} else {
			document.querySelector("html")?.classList.remove("dark");
			this.theme = "light";
		}
	}

	toggleTheme(): void {
		document.querySelector("html")?.classList.toggle("dark");
		this.theme = this.theme == "light" ? "dark" : "light";
		localStorage.setItem("theme", this.theme);
	}
}
