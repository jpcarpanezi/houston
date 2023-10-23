import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ConnectorFunctionUseCaseInterface } from 'src/app/domain/interfaces/use-cases/connector-function-use-case.interface';
import { ConnectorFunctionViewModel } from 'src/app/domain/view-models/connector-function.view-model';
import { Toast } from 'src/app/infra/helpers/toast';
import Swal from 'sweetalert2';
import { ModalComponent } from '../../modal/modal.component';

@Component({
	selector: 'app-new-connector-function',
	templateUrl: './new-connector-function.component.html',
	styleUrls: ['./new-connector-function.component.css']
})
export class NewConnectorFunctionComponent {
	@ViewChild("newConnectorFunction") public newConnectorFunctionModal?: ModalComponent;
	@Output("onSubmit") public onSubmit: EventEmitter<ConnectorFunctionViewModel> = new EventEmitter<ConnectorFunctionViewModel>();

	private connectorId?: string;

	public createConnectorFunctionForm: FormGroup = this.initializeConnectorFunctionForm();

	constructor(
		private fb: FormBuilder,
		private connectorFunctionUseCase: ConnectorFunctionUseCaseInterface
	) { }

	private initializeConnectorFunctionForm(): FormGroup {
		return this.createConnectorFunctionForm = this.fb.group({
			name: ["", [
				Validators.required,
				Validators.maxLength(50)
			]],
			description: [null, [
				Validators.maxLength(5000)
			]]
		});
	}

	createConnectorFunction(): void {
		if (this.createConnectorFunctionForm.invalid) return;
		this.createConnectorFunctionForm.disable();

		this.createConnectorFunctionForm.value['connectorId'] = this.connectorId;
		this.connectorFunctionUseCase.create(this.createConnectorFunctionForm.value).subscribe({
			next: (response: ConnectorFunctionViewModel) => this.createConnectorFunctionNext(response),
			error: () => Swal.fire("Error", "An error has occurred while trying to create the connector function.", "error")
		}).add(() => {
			this.createConnectorFunctionForm.enable();
			this.newConnectorFunctionModal?.close();
		});
	}

	createConnectorFunctionNext(response: ConnectorFunctionViewModel): void {
		Toast.fire({
			icon: "success",
			title: "Connector function created."
		});

		this.onSubmit.emit(response);
	}

	open(connectorId: string): void {
		this.connectorId = connectorId;
		this.createConnectorFunctionForm.reset();
		this.newConnectorFunctionModal?.open();
	}
}
