import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { ModalComponent } from '../../../shared/modal/modal.component';
import { UserViewModel } from 'src/app/domain/view-models/user.view-model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CustomValidators } from 'src/app/infra/helpers/custom-validators.helper';
import { UserUseCaseInterface } from 'src/app/domain/interfaces/use-cases/user-use-case.interface';
import Swal from 'sweetalert2';
import { Toast } from 'src/app/infra/helpers/toast';

@Component({
	selector: 'app-new-user',
	templateUrl: './new-user.component.html',
	styleUrls: ['./new-user.component.css']
})
export class NewUserComponent {
	@ViewChild("newUser") public newPipelineModal?: ModalComponent;
	@Output("onCreate") public submitResponse: EventEmitter<UserViewModel> = new EventEmitter<UserViewModel>();

	public createUserForm: FormGroup = this.initializeCreateUserForm();

	constructor(
		private fb: FormBuilder,
		private userUseCase: UserUseCaseInterface
	) { }

	private initializeCreateUserForm(): FormGroup {
		return this.createUserForm = this.fb.group({
			name: ["", [
				Validators.required,
				Validators.maxLength(240)
			]],
			email: ["", [
				Validators.required,
				Validators.email
			]],
			userRole: ["User", [
				Validators.required
			]],
			tempPassword: ["", [
				Validators.minLength(8),
				Validators.maxLength(64),
				Validators.pattern(/^(?=.*[A-Z])(?=.*\d).+$/),
				CustomValidators.fieldMatch("confirmTempPassword", true)
			]],
			confirmTempPassword: ["", [
				Validators.required,
				CustomValidators.fieldMatch("tempPassword")
			]]
		});
	}

	createUser(): void {
		if (this.createUserForm.invalid) return;
		this.createUserForm.disable();

		this.userUseCase.create(this.createUserForm.value).subscribe({
			next: (response: UserViewModel) => {
				Toast.fire({
					icon: "success",
					title: "Pipeline created."
				});

				this.submitResponse.emit(response);
			},
			error: () => Swal.fire("Error", "An error has occurred while trying to create the user.", "error")
		}).add(() => {
			this.createUserForm.enable();
			this.newPipelineModal?.close();
		})
	}

	open(): void {
		this.newPipelineModal?.open();
	}
}
