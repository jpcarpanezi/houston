import { trigger, transition, style, animate } from '@angular/animations';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserUseCaseInterface } from 'src/app/domain/interfaces/use-cases/user-use-case.interface';
import { CustomValidators } from 'src/app/infra/helpers/custom-validators.helper';
import Swal from 'sweetalert2';

@Component({
	selector: 'app-first-access',
	templateUrl: './first-access.component.html',
	styleUrls: ['./first-access.component.css'],
	animations: [
		trigger('fadeInOut', [
			transition(':enter', [   // :enter is alias to 'void => *'
				style({ opacity: 0 }),
				animate(500, style({ opacity: 1 }))
			]),
			transition(':leave', [   // :leave is alias to '* => void'
				animate(500, style({ opacity: 0 }))
			])
		])
	],
})
export class FirstAccessComponent implements OnInit {
	public firstAccessForm: FormGroup = this.initializeFirstAccessForm();
	public showPasswordMatch: boolean = false;

	constructor(
		private fb: FormBuilder,
		private route: ActivatedRoute,
		private router: Router,
		private useUserCase: UserUseCaseInterface
	) { }

	ngOnInit(): void {
		if (!this.route.snapshot.queryParams["email"]) {
			this.router.navigateByUrl("/login");
		}
	}

	private initializeFirstAccessForm(): FormGroup {
		return this.firstAccessForm = this.fb.group({
			password: ["", [
				Validators.required,
				Validators.minLength(8),
				Validators.maxLength(64),
				Validators.pattern(/^(?=.*[A-Z])(?=.*\d).+$/),
				CustomValidators.fieldMatch("confirmPassword", true)
			]],
			confirmPassword: ["", [
				Validators.required,
				CustomValidators.fieldMatch("password")
			]]
		});
	}

	firstAccess(): void {
		if (this.firstAccessForm.invalid) return;
		this.firstAccessForm.disable();
		this.firstAccessForm.value["email"] = this.route.snapshot.queryParams["email"];
		this.firstAccessForm.value["token"] = this.route.snapshot.paramMap.get("token");

		this.useUserCase.firstAccess(this.firstAccessForm.value).subscribe({
			next: () => Swal.fire("Success", "Your password has been updated.", "success").then(() => this.router.navigateByUrl("/login")),
			error: (error: HttpErrorResponse[]) => this.firstAccessError(error[0])
		}).add(() => this.firstAccessForm.enable());
	}

	firstAccessError(error: HttpErrorResponse): void {
		switch (error.status) {
			case HttpStatusCode.BadRequest:
				Swal.fire(
					"Bad request",
					"New password could not be equal to the old one.",
					"error"
				);
				break;
			case HttpStatusCode.Forbidden:
				Swal.fire(
					"Invalid token",
					"Your token is invalid or expired.",
					"error"
				);
				break;
			default:
				Swal.fire(
					"Something went wrong",
					"Try again later.",
					"error"
				);
				break;
		}
	}
}
