import { Component } from '@angular/core';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent {
	public isSettingsOpen: boolean = false;

	toggleSettings(): void {
		this.isSettingsOpen = !this.isSettingsOpen;
	}
}
