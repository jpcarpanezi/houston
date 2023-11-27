import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ConnectorUseCaseInterface } from 'src/app/domain/interfaces/use-cases/connector-use-case.interface';
import { ConnectorViewModel } from 'src/app/domain/view-models/connector.view-model';
import { Toast } from 'src/app/infra/helpers/toast';
import Swal from 'sweetalert2';
import { ModalComponent } from '../../modal/modal.component';

@Component({
	selector: 'app-new-connector',
	templateUrl: './new-connector.component.html',
	styleUrls: ['./new-connector.component.css']
})
export class NewConnectorComponent implements OnInit {
	@ViewChild("newConnector") public newConnectorModal?: ModalComponent;
	@Output("submitResponse") public submitResponse: EventEmitter<ConnectorViewModel> = new EventEmitter<ConnectorViewModel>();

	public createConnectorForm: FormGroup = this.initializeNewConnectorForm();
	private nameFieldChanged: boolean = false;

	constructor(
		private fb: FormBuilder,
		private connectorUseCase: ConnectorUseCaseInterface
	) { }

	ngOnInit(): void {
		// this.kebabCaseConverter();
	}

	initializeNewConnectorForm(): FormGroup {
		return this.createConnectorForm = this.fb.group({
			friendlyName: ["", [
				Validators.required,
				Validators.maxLength(50)
			]],
			name: ["", [
				Validators.required,
				Validators.maxLength(64),
				Validators.pattern("^[a-z][a-z0-9-]*$")
			]],
			description: [null, [
				Validators.maxLength(5000)
			]],
		});
	}

	private kebabCaseConverter(): void {
		this.createConnectorForm.get("friendlyName")?.valueChanges.subscribe((value: string) => {
			this.createConnectorForm.get("name")?.setValue(this.convertToKebabCase(value));
		});
	}

	private convertToKebabCase(value: string): string {
		if (!value) return "";

		const converted = value
			.normalize("NFD")
			.replace(/[\u0300-\u036f]/g, '')
			.replace(/-+/g, '-')
			.replace(/\s+/g, '-')
			.toLowerCase();

		return converted;
	}

	createConnector(): void {
		if (this.createConnectorForm.invalid) return;
		this.createConnectorForm.disable();

		console.log(this.createConnectorForm.value);

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
