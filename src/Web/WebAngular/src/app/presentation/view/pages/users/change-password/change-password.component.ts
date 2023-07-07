import { Component, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserUseCaseInterface } from 'src/app/domain/interfaces/use-cases/user-use-case.interface';
import { CustomValidators } from 'src/app/infra/helpers/custom-validators.helper';
import { ModalComponent } from '../../../shared/modal/modal.component';
import { UpdatePasswordCommand } from 'src/app/domain/commands/user-commands/update-password.command';
import Swal from 'sweetalert2';
import { Toast } from 'src/app/infra/helpers/toast';

@Component({
	selector: 'app-change-password',
	templateUrl: './change-password.component.html',
	styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent {
	@ViewChild("changePasswordModal") public changePasswordModal?: ModalComponent;

	public changePasswordForm: FormGroup = this.initializeChangePasswordForm();
	private userId?: string;

	constructor(
		private fb: FormBuilder,
		private userUseCase: UserUseCaseInterface
	) { }

	private initializeChangePasswordForm(): FormGroup {
		return this.changePasswordForm = this.fb.group({
			newPassword: ["", [
				Validators.required,
				Validators.minLength(8),
				Validators.maxLength(64),
				Validators.pattern(/^(?=.*[A-Z])(?=.*\d).+$/),
				CustomValidators.fieldMatch("confirmNewPassword", true)
			]],
			confirmNewPassword: ["", [
				Validators.required,
				CustomValidators.fieldMatch("newPassword")
			]]
		});
	}

	changePassword(): void {
		if (this.changePasswordForm.invalid) return;
		this.changePasswordForm.disable();

		const command: UpdatePasswordCommand = {
			userId: this.userId!,
			newPassword: this.changePasswordForm.value.newPassword
		};

		this.userUseCase.changePassword(command).subscribe({
			next: () => Toast.fire({ icon: "success", title: "Password changed." }),
			error: () => Swal.fire("Error", "An error has occurred while trying to change the password.", "error")
		}).add(() => {
			this.changePasswordForm.enable();
			this.changePasswordModal?.close();
		})
	}

	open(id: string): void {
		this.userId = id;
		this.changePasswordModal?.open();
	}
}
