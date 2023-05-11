import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthUseCaseInterface } from 'src/app/domain/interfaces/use-cases/auth-use-case.interface';
import { BearerTokenViewModel } from 'src/app/domain/view-models/bearer-token.view-model';
import { AuthService } from 'src/app/infra/auth/auth.service';

@Component({
	selector: 'app-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.css']
})
export class LoginComponent {
	public loginForm: FormGroup = this.initializeLoginForm();

	constructor(
		private fb: FormBuilder,
		private authUseCase: AuthUseCaseInterface,
		private authService: AuthService,
		private router: Router
	) { }

	initializeLoginForm(): FormGroup {
		return this.loginForm = this.fb.group({
			email: ["", Validators.required],
			password: ["", Validators.required]
		});
	}

	signIn(): void {
		this.loginForm.disable();

		this.authUseCase.signIn(this.loginForm.value).subscribe({
			next: (response: BearerTokenViewModel) => this.signInResponse(response)
		}).add(() => this.loginForm.enable());
	}

	private signInResponse(response: BearerTokenViewModel): void {
		this.authService.setSession(response);
		this.router.navigateByUrl("/home");
	}
}
