import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/infra/auth/auth.service';
import { UserSessionViewModel } from 'src/app/domain/view-models/user-session.view-model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
	public userInfo: UserSessionViewModel | null = null;

	constructor(
		private authService: AuthService
	) { }

	ngOnInit(): void {
		this.userInfo = this.authService.userInfo;
	}
}
