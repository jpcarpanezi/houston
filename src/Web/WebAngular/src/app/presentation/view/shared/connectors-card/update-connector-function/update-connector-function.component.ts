import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ConnectorFunctionUseCaseInterface } from 'src/app/domain/interfaces/use-cases/connector-function-use-case.interface';
import { ConnectorFunctionViewModel } from 'src/app/domain/view-models/connector-function.view-model';
import { Toast } from 'src/app/infra/helpers/toast';
import Swal from 'sweetalert2';
import { ModalComponent } from '../../modal/modal.component';

@Component({
	selector: 'app-update-connector-function',
	templateUrl: './update-connector-function.component.html',
	styleUrls: ['./update-connector-function.component.css']
})
export class UpdateConnectorFunctionComponent {
	@Output("onSubmit") public onSubmit: EventEmitter<ConnectorFunctionViewModel> = new EventEmitter<ConnectorFunctionViewModel>();
	@ViewChild("updateConnectorFunctionModal") public updateConnectorFunctionModal?: ModalComponent;

	public updateConnectorFunctionForm: FormGroup = this.initializeUpdateConnectorFunctionForm();

	public connectorFunction?: ConnectorFunctionViewModel;

	constructor(
		private fb: FormBuilder,
		private connectorFunctionUseCase: ConnectorFunctionUseCaseInterface
	) { }

	private initializeUpdateConnectorFunctionForm(): FormGroup {
		return this.updateConnectorFunctionForm = this.fb.group({
			name: ["", [
				Validators.required,
				Validators.maxLength(50)
			]],
			description: [null, [
				Validators.maxLength(5000)
			]]
		});
	}

	updateConnectorFunction(): void {
		if (this.updateConnectorFunctionForm.invalid) return;
		this.updateConnectorFunctionForm.disable();

		this.updateConnectorFunctionForm.value["id"] = this.connectorFunction?.id;
		this.connectorFunctionUseCase.update(this.updateConnectorFunctionForm.value).subscribe({
			next: (response: ConnectorFunctionViewModel) => this.updateConnectorFunctionNext(response),
			error: () => Swal.fire("Error", "An error has occurred while trying to update the connector function.", "error")
		}).add(() => {
			this.updateConnectorFunctionForm.enable();
			this.updateConnectorFunctionModal?.close();
		});
	}

	private updateConnectorFunctionNext(response: ConnectorFunctionViewModel): void {
		Toast.fire({
			icon: "success",
			title: "Connector function updated."
		});

		this.onSubmit.emit(response);
	}

	open(connectorFunction: ConnectorFunctionViewModel): void {
		this.connectorFunction = connectorFunction;
		this.updateConnectorFunctionForm.patchValue(connectorFunction);
		this.updateConnectorFunctionModal?.open();
	}
}
