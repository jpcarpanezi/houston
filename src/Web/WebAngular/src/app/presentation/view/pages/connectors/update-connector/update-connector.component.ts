import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { ModalComponent } from '../../../shared/modal/modal.component';
import { ConnectorViewModel } from 'src/app/domain/view-models/connector.view-model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConnectorUseCaseInterface } from 'src/app/domain/interfaces/use-cases/connector-use-case.interface';
import Swal from 'sweetalert2';
import { Toast } from 'src/app/infra/helpers/toast';

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
			name: ["", [
				Validators.required,
				Validators.maxLength(50)
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
