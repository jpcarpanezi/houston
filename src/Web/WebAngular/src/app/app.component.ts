import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
	public isSidebarEnabled: boolean = false;

	constructor(
		private router: Router
	) { }

	ngOnInit(): void {
		this.router.events.subscribe(event => {
			if (event instanceof NavigationEnd) {
				const currentRoute = this.router.routerState.root.firstChild?.snapshot;
				const toolbarEnabled = currentRoute?.data["toolbar"] ?? true;
				this.isSidebarEnabled = toolbarEnabled;
			}
		});
	}
}
