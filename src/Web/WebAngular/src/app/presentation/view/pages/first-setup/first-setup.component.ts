import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserUseCaseInterface } from 'src/app/domain/interfaces/use-cases/user-use-case.interface';
import { CustomValidators } from 'src/app/infra/helpers/custom-validators.helper';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-first-setup',
  templateUrl: './first-setup.component.html',
  styleUrls: ['./first-setup.component.css']
})
export class FirstSetupComponent {
	public firstSetupForm: FormGroup = this.initializeFirstSetupForm();
	public firstStep: boolean = true;

	constructor(
		private fb: FormBuilder,
		private userUseCase: UserUseCaseInterface,
		private router: Router
	) { }

	initializeFirstSetupForm(): FormGroup {
		return this.fb.group({
			registryAddress: ["hub.docker.com", Validators.required],
			registryEmail: ["", [
				Validators.required,
				Validators.email
			]],
			registryUsername: ["", [
				Validators.required,
				Validators.maxLength(50)
			]],
			registryPassword: ["", [
				Validators.required,
				Validators.maxLength(64)
			]],
			userName: ["", [
				Validators.required,
				Validators.maxLength(240)
			]],
			userEmail: ["", [
				Validators.required,
				Validators.email
			]],
			userPassword: ["", [
				Validators.required,
				Validators.minLength(8),
				Validators.maxLength(64),
				Validators.pattern(/^(?=.*[A-Z])(?=.*\d).+$/),
				CustomValidators.fieldMatch("confirmPassword", true)
			]],
			confirmPassword: ["", [
				Validators.required,
				CustomValidators.fieldMatch("userPassword")
			]]
		});
	}

	changeStep(): void {
		if (this.firstStep) {
			const registryAddressErrors = this.firstSetupForm.controls["registryAddress"].errors;
			const registryEmailErrors = this.firstSetupForm.controls["registryEmail"].errors;
			const registryUsernameErrors = this.firstSetupForm.controls["registryUsername"].errors;
			const registryPasswordErrors = this.firstSetupForm.controls["registryPassword"].errors;

			if (registryAddressErrors || registryEmailErrors || registryUsernameErrors || registryPasswordErrors) {
				this.firstSetupForm.controls["registryAddress"].markAsTouched();
				this.firstSetupForm.controls["registryEmail"].markAsTouched();
				this.firstSetupForm.controls["registryUsername"].markAsTouched();
				this.firstSetupForm.controls["registryPassword"].markAsTouched();

				return;
			}
		}

		this.firstStep = !this.firstStep;
	}

	firstSetup(): void {
		if (this.firstSetupForm.invalid) return;
		this.firstSetupForm.disable();

		this.userUseCase.firstSetup(this.firstSetupForm.value).subscribe({
			next: () => Swal.fire("Success", "First setup completed successfully.", "success").then(() => this.router.navigateByUrl("/login")),
			error: () => Swal.fire("Error", "The system has already been set up and configured.", "error")
		}).add(() => this.firstSetupForm.enable());
	}
}
