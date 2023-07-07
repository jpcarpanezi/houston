import { Injectable } from '@angular/core';
import { Route, Routes } from '@angular/router';
import { BaseComponent } from '../base/base.component';
import { authGuard } from 'src/app/infra/auth/auth.guard';

@Injectable({
	providedIn: 'root'
})
export class RouteService {
	constructor() { }

	static withShell(routes: Routes): Route {
		return {
			path: "",
			component: BaseComponent,
			children: routes,
			canActivate: [authGuard]
		}
	}
}
