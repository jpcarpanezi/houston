import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
	selector: 'app-sidebar',
	templateUrl: './sidebar.component.html',
	styleUrls: ['./sidebar.component.css'],
	encapsulation: ViewEncapsulation.None
})
export class SidebarComponent implements OnInit {
	public isSidebarOpen: boolean = true;
	@Input("enabled") isSidebarEnabled: boolean = true;

	ngOnInit(): void {
		const isSidebarOpen = localStorage.getItem("isSidebarOpen");
		if (isSidebarOpen == null) {
			localStorage.setItem("isSidebarOpen", "true");
		}

		localStorage.getItem("isSidebarOpen") == "true" ? this.isSidebarOpen = true : this.isSidebarOpen = false;
	}

	toggleSidebar(): void {
		this.isSidebarOpen = !this.isSidebarOpen;
		localStorage.setItem("isSidebarOpen", this.isSidebarOpen.toString());
	}
}
