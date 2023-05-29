import { Component } from '@angular/core';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css']
})
export class NotificationsComponent {
	public isNotificationsOpen: boolean = false;

	toggleNotifications(): void {
		this.isNotificationsOpen = !this.isNotificationsOpen;
	}
}
