import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
	public isSidebarOpen: boolean = true;
	public isToolbarEnabled: boolean = false;

	constructor(
		private router: Router
	) { }

	ngOnInit(): void {
		this.router.events.subscribe(event => {
			if (event instanceof NavigationEnd) {
				const currentRoute = this.router.routerState.root.firstChild?.snapshot;
				const toolbarEnabled = currentRoute?.data["toolbar"] ?? true;
				this.isToolbarEnabled = toolbarEnabled;
				console.log("isToolbarEnabled", this.isToolbarEnabled);
				console.log("isSidebarOpen", this.isSidebarOpen);
				console.log(!(this.isSidebarOpen && this.isToolbarEnabled))
			}
		});
	}

	toggleSidebar(): void {
		console.log(this.isSidebarOpen);
		this.isSidebarOpen = !this.isSidebarOpen;
	}
}
