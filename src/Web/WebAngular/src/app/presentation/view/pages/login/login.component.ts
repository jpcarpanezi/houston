import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthUseCaseInterface } from 'src/app/domain/interfaces/use-cases/auth-use-case.interface';
import { UserUseCaseInterface } from 'src/app/domain/interfaces/use-cases/user-use-case.interface';
import { BearerTokenViewModel } from 'src/app/domain/view-models/bearer-token.view-model';
import { AuthService } from 'src/app/infra/auth/auth.service';
import Swal from 'sweetalert2';

@Component({
	selector: 'app-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
	public loginForm: FormGroup = this.initializeLoginForm();
	public isLoading: boolean = true;

	constructor(
		private fb: FormBuilder,
		private authUseCase: AuthUseCaseInterface,
		private userUseCase: UserUseCaseInterface,
		private authService: AuthService,
		private router: Router
	) { }

	ngOnInit(): void {
		this.userUseCase.isFirstSetup().subscribe({
			error: (error: HttpErrorResponse[]) => {
				if (error[0].error["errorCode"] == "firstSetup") {
					Swal.fire("First setup detected", "Seems it's the first time you are here, let's start from the beginning.", "info").then(() => this.router.navigateByUrl("/first-setup"))
				} else {
					Swal.fire("Something went wrong", "Try again later.", "error");
				}
			}
		}).add(() => this.isLoading = false);
	}

	initializeLoginForm(): FormGroup {
		return this.loginForm = this.fb.group({
			email: ["", [Validators.required, Validators.email]],
			password: ["", Validators.required]
		});
	}

	signIn(): void {
		if (this.loginForm.invalid) return;
		this.loginForm.disable();

		this.authUseCase.signIn(this.loginForm.value).subscribe({
			next: (response: BearerTokenViewModel) => this.signInResponse(response),
			error: (error: HttpErrorResponse[]) => this.signError(error[0])
		}).add(() => this.loginForm.enable());
	}

	isFieldInvalid(field: string): boolean | null {
		return this.loginForm.controls[field].errors && this.loginForm.controls[field].touched;
	}

	private signError(error: HttpErrorResponse) {
		switch (error.status) {
			case HttpStatusCode.TemporaryRedirect:
				Swal.fire(
					"First access required",
					"You need to define a password for your account.",
					"info"
				).then(() => this.router.navigate(
					[`/first-access/${error.error["passwordToken"]}`],
					{ queryParams: { email: this.loginForm.value.email } }
				));
				break;
			case HttpStatusCode.Forbidden:
				Swal.fire(
					"Invalid credentials",
					"Check your email and password and try again.",
					"error"
				);
				break
			default:
				Swal.fire(
					"Something went wrong",
					"Try again later.",
					"error"
				);
				break;
		}
	}

	private signInResponse(response: BearerTokenViewModel): void {
		const returnUrl = this.router.parseUrl(this.router.url).queryParams["returnUrl"];

		this.authService.setSession(response);

		if (returnUrl) {
			this.router.navigateByUrl(returnUrl);
		} else {
			this.router.navigateByUrl("/home");
		}
	}
}
