import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ConnectorUseCaseInterface } from 'src/app/domain/interfaces/use-cases/connector-use-case.interface';
import { ConnectorViewModel } from 'src/app/domain/view-models/connector.view-model';
import { Toast } from 'src/app/infra/helpers/toast';
import Swal from 'sweetalert2';
import { ModalComponent } from '../../modal/modal.component';

@Component({
	selector: 'app-update-connector',
	templateUrl: './update-connector.component.html',
	styleUrls: ['./update-connector.component.css']
})
export class UpdateConnectorComponent {
	@ViewChild("updateConnectorModal") public updateConnectorModal?: ModalComponent;
	@Output("onUpdate") public onUpdate: EventEmitter<ConnectorViewModel> = new EventEmitter<ConnectorViewModel>();

	public updateConnectorForm: FormGroup = this.initializeUpdateConnectorForm();
	public connector?: ConnectorViewModel;

	constructor(
		private fb: FormBuilder,
		private connectorUseCase: ConnectorUseCaseInterface
	) { }

	initializeUpdateConnectorForm(): FormGroup {
		return this.updateConnectorForm = this.fb.group({
			friendlyName: ["", [
				Validators.required,
				Validators.maxLength(50)
			]],
			name: [this.connector?.name, [
				Validators.required,
				Validators.maxLength(64),
				Validators.pattern("^[a-z][a-z0-9-]*$")
			]],
			description: ["", [
				Validators.maxLength(5000)
			]]
		});
	}

	updateConnector(): void {
		if (this.updateConnectorForm.invalid) return;
		this.updateConnectorForm.disable();

		this.updateConnectorForm.value["connectorId"] = this.connector?.id;
		this.connectorUseCase.update(this.updateConnectorForm.value).subscribe({
			next: (response: ConnectorViewModel) => this.updateConnectorNext(response),
			error: () => Swal.fire("Error", "An error has occurred while trying to update the connector.", "error")
		}).add(() => {
			this.updateConnectorForm.enable();
			this.updateConnectorModal?.close();
		});

	}

	private updateConnectorNext(response: ConnectorViewModel): void {
		Toast.fire({
			icon: "success",
			title: "Connector updated."
		});

		this.onUpdate.emit(response);
	}

	open(connector: ConnectorViewModel): void {
		this.connector = connector;
		this.updateConnectorForm.patchValue(connector);
		this.updateConnectorModal?.open();
	}
}
