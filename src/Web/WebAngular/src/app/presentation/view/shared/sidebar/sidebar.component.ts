import { Component, Input, ViewEncapsulation } from '@angular/core';

@Component({
	selector: 'app-sidebar',
	templateUrl: './sidebar.component.html',
	styleUrls: ['./sidebar.component.css'],
	encapsulation: ViewEncapsulation.None
})
export class SidebarComponent {
	public isSidebarOpen: boolean = true;
	@Input("enabled") isSidebarEnabled: boolean = true;

	toggleSidebar(): void {
		this.isSidebarOpen = !this.isSidebarOpen;
	}
}
