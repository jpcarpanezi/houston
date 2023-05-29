import { Component, ElementRef, HostListener, Input } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/infra/auth/auth.service';
import { NotificationsComponent } from '../notifications/notifications.component';
import { SearchBarComponent } from '../search-bar/search-bar.component';
import { SettingsComponent } from '../settings/settings.component';

@Component({
  selector: 'app-mini-menu',
  templateUrl: './mini-menu.component.html',
  styleUrls: ['./mini-menu.component.css']
})
export class MiniMenuComponent {
	@Input("notificationsPanelRef") notificationsRef?: NotificationsComponent;
	@Input("searchPanelRef") searchRef?: SearchBarComponent;
	@Input("settingsPanelRef") settingsPanelRef?: SettingsComponent;

	public isUserMenuOpen: boolean = false;

	constructor(
		private elementRef: ElementRef,
		private authService: AuthService,
		private router: Router
	) { }

	@HostListener("document:click", ["$event.target"])
	onClick(target: any) {
		const clickedInside = this.elementRef.nativeElement.contains(target);

		if (!clickedInside) {
			this.isUserMenuOpen = false;
		}
	}

	toggleUserMenu(): void {
		this.isUserMenuOpen = !this.isUserMenuOpen;
	}

	toggleNotificationsPanel(): void {
		this.notificationsRef?.toggleNotifications();
	}

	toggleSearchPanel(): void {
		this.searchRef?.toggleSearchBar();
	}

	toggleSettingsPanel(): void {
		this.settingsPanelRef?.toggleSettings();
	}

	signOut(event: Event): void {
		event.preventDefault();
		this.authService.removeSession();
		this.router.navigateByUrl("/login");
	}
}
