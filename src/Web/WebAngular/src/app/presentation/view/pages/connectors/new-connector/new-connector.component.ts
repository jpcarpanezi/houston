import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { ModalComponent } from '../../../shared/modal/modal.component';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConnectorViewModel } from 'src/app/domain/view-models/connector.view-model';
import { ConnectorUseCaseInterface } from 'src/app/domain/interfaces/use-cases/connector-use-case.interface';
import Swal from 'sweetalert2';
import { Toast } from 'src/app/infra/helpers/toast';

@Component({
	selector: 'app-new-connector',
	templateUrl: './new-connector.component.html',
	styleUrls: ['./new-connector.component.css']
})
export class NewConnectorComponent {
	@ViewChild("newConnector") public newConnectorModal?: ModalComponent;
	@Output("submitResponse") public submitResponse: EventEmitter<ConnectorViewModel> = new EventEmitter<ConnectorViewModel>();

	public createConnectorForm: FormGroup = this.initializeNewConnectorForm();

	constructor(
		private fb: FormBuilder,
		private connectorUseCase: ConnectorUseCaseInterface
	) { }

	initializeNewConnectorForm(): FormGroup {
		return this.createConnectorForm = this.fb.group({
			name: ["", [
				Validators.required,
				Validators.maxLength(50)
			]],
			description: [null, [
				Validators.maxLength(5000)
			]],
		});
	}

	createConnector(): void {
		if (this.createConnectorForm.invalid) return;
		this.createConnectorForm.disable();

		this.connectorUseCase.create(this.createConnectorForm.value).subscribe({
			next: (response: ConnectorViewModel) => this.createConnectorNext(response),
			error: () => Swal.fire("Error", "An error has occurred while trying to create the connector.", "error")
		}).add(() => {
			this.createConnectorForm.enable();
			this.newConnectorModal?.close();
		});
	}

	private createConnectorNext(response: ConnectorViewModel): void {
		Toast.fire({
			icon: "success",
			title: "Connector created."
		});

		this.submitResponse.emit(response);
	}

	open(): void {
		this.newConnectorModal?.open();
	}
}
