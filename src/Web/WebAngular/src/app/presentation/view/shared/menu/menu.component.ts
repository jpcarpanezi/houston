import { Component, OnInit } from '@angular/core';
import { UserSessionViewModel } from 'src/app/domain/view-models/user-session.view-model';
import { AuthService } from 'src/app/infra/auth/auth.service';

@Component({
	selector: 'app-menu',
	templateUrl: './menu.component.html',
	styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
	public user: UserSessionViewModel | null = null;

	constructor(
		private authService: AuthService
	) { }

	ngOnInit(): void {
		this.authService.userInfoSubject.subscribe((userInfo: UserSessionViewModel | null) => {
			this.user = userInfo;
		});
	}
}
