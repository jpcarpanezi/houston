import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CodemirrorComponent } from '@ctrl/ngx-codemirror';
import { Subscription, interval, startWith, switchMap, takeWhile } from 'rxjs';
import { BuildStatus } from 'src/app/domain/enums/build-status.enum';
import { ConnectorFunctionUseCaseInterface } from 'src/app/domain/interfaces/use-cases/connector-function-use-case.interface';
import { ConnectorFunctionInputViewModel } from 'src/app/domain/view-models/connector-function-input.view-model';
import { ConnectorFunctionViewModel } from 'src/app/domain/view-models/connector-function.view-model';
import { Toast } from 'src/app/infra/helpers/toast';
import Swal from 'sweetalert2';
import { ConnectorFunctionStderrComponent } from './connector-function-stderr/connector-function-stderr.component';

@Component({
	selector: 'app-connector-function',
	templateUrl: './connector-function.component.html',
	styleUrls: ['./connector-function.component.css']
})
export class ConnectorFunctionComponent implements OnInit, OnDestroy {
	@ViewChild("codeEditor") private codeEditor?: CodemirrorComponent;
	@ViewChild("packageEditor") private packageEditor?: CodemirrorComponent;

	public isLoading: boolean = false;
	public connectorFunctionForm: FormGroup = this.initializeConnectorFunctionForm();

	public connectorFunction?: ConnectorFunctionViewModel;
	public connectorId?: string;
	public connectorFunctionId?: string | null;

	public buildStatus?: BuildStatus;
	public longPooling?: Subscription;

	constructor(
		private fb: FormBuilder,
		private route: ActivatedRoute,
		private router: Router,
		private connectorFunctionUseCase: ConnectorFunctionUseCaseInterface
	) { }

	ngOnInit(): void {
		this.connectorId = this.route.snapshot.queryParams["connectorId"];
		this.connectorFunctionId = this.route.snapshot.paramMap.get("id");

		if (!this.connectorId && !this.connectorFunctionId) {
			this.router.navigate(["/connectors"]);
		}

		if (this.connectorFunctionId) {
			this.getConnectorFunction(this.connectorFunctionId);
			this.triggerLongPooling();
		}
	}

	ngOnDestroy(): void {
		this.longPooling?.unsubscribe();
	}

	private triggerLongPooling(): void {
		this.longPooling = interval(5000).pipe(
			startWith(0),
			switchMap(() => this.connectorFunctionUseCase.get(this.connectorFunctionId!)),
			takeWhile(x => x.buildStatus !== "Success" && x.buildStatus !== "Failed", true)
		).subscribe({
			next: (response: ConnectorFunctionViewModel) => this.buildStatus = response.buildStatus,
			error: () => Swal.fire("Error", "An error has occurred while trying to get the connector function.", "error")
		});
	}

	private initializeConnectorFunctionForm(): FormGroup {
		return this.connectorFunctionForm = this.fb.group({
			name: ["", [
				Validators.required,
				Validators.maxLength(50)
			]],
			description: ["", [
				Validators.maxLength(5000)
			]],
			inputs: this.fb.array([]),
		});
	}

	get inputs(): FormArray {
		return this.connectorFunctionForm.get("inputs") as FormArray;
	}

	getInputValues(control: AbstractControl): FormArray {
		return control.get("values") as FormArray;
	}

	getInputDefaultValue(control: AbstractControl): AbstractControl {
		return control.get("defaultValue") as AbstractControl;
	}

	private getConnectorFunction(connectorFunctionId: string): void {
		this.isLoading = true;

		this.connectorFunctionUseCase.get(connectorFunctionId).subscribe({
			next: (response: ConnectorFunctionViewModel) => this.getConnectorFunctionNext(response),
			error: () => Swal.fire("Error", "An error has occurred while getting the connector function.", "error")
		}).add(() => this.isLoading = false);
	}

	private getConnectorFunctionNext(response: ConnectorFunctionViewModel): void {
		this.connectorFunction = response;
		this.buildStatus = response.buildStatus;

		response.inputs?.forEach((input: ConnectorFunctionInputViewModel) => {
			this.addInput();

			input.values?.forEach(() => {
				this.addInputValue(this.inputs.controls[this.inputs.controls.length - 1]);
			});
		});

		this.connectorFunctionForm.patchValue(response);
		this.codeEditor?.codeMirror?.getDoc().setValue(window.atob(response.script as string));
		this.packageEditor?.codeMirror?.getDoc().setValue(window.atob(response.package as string));
	}

	addInput(): void {
		const inputFormGroup = this.fb.group({
			inputType: ["", [
				Validators.required
			]],
			name: ["", [
				Validators.required,
				Validators.maxLength(25)
			]],
			placeholder: ["", [
				Validators.required,
				Validators.maxLength(50)
			]],
			replace: ["", [
				Validators.required,
				Validators.maxLength(25)
			]],
			values: this.fb.array([]),
			defaultValue: [null, [
				Validators.maxLength(25)
			]],
			required: [false],
			advancedOption: [false]
		});

		this.inputs.push(inputFormGroup);
	}

	removeInput(index: number): void {
		this.inputs.removeAt(index);
	}

	addInputValue(control: AbstractControl): void {
		const inputValueFormControl = this.fb.control("", [
			Validators.required,
			Validators.maxLength(25)
		]);

		this.getInputValues(control).push(inputValueFormControl);
	}

	removeInputValue(control: AbstractControl, index: number): void {
		this.getInputValues(control).removeAt(index);
	}

	removeAllInputValues(control: AbstractControl): void {
		this.getInputValues(control).clear();
	}

	saveConnectorFunction(): void {
		if (this.connectorFunctionForm.invalid) return;
		this.connectorFunctionForm.disable();

		const script: string | undefined = window.btoa(this.codeEditor?.codeMirror?.getDoc().getValue()!);
		const packageJson: string | undefined = window.btoa(this.packageEditor?.codeMirror?.getDoc().getValue()!);
		this.connectorFunctionForm.value["connectorId"] = this.connectorId;
		this.connectorFunctionForm.value["script"] = script;
		this.connectorFunctionForm.value["package"] = packageJson;
		this.connectorFunctionForm.value["version"] = "1.0.0";

		if (this.connectorFunction) {
			this.connectorFunctionForm.value["id"] = this.connectorFunction.id;

			this.connectorFunctionUseCase.update(this.connectorFunctionForm.value).subscribe({
				next: (response: ConnectorFunctionViewModel) => {
					Toast.fire({
						icon: "success",
						title: "Updated successfully."
					});

					this.buildStatus = response.buildStatus;
					this.connectorFunction = response;
					this.triggerLongPooling();
				},
				error: () => Swal.fire("Error", "An error has occurred while updating the connector function.", "error")
			}).add(() => this.connectorFunctionForm.enable());
		} else {
			this.connectorFunctionUseCase.create(this.connectorFunctionForm.value).subscribe({
				next: (response: ConnectorFunctionViewModel) => this.router.navigate(["/connector-function", response.id]),
				error: () => Swal.fire("Error", "An error has occurred while creating the connector function.", "error")
			}).add(() => this.connectorFunctionForm.enable());
		}
	}

	deleteConnectorFunction(): void {
		Swal.fire({
			icon: "question",
			title: "Are you sure?",
			text: "You won't be able to revert this!",
			showDenyButton: true,
			showConfirmButton: true,
			confirmButtonText: "Yes, delete it",
			denyButtonText: "No, cancel",
		}).then((result) => {
			if (result.isConfirmed) {
				this.isLoading = true;

				this.connectorFunctionUseCase.delete(this.connectorFunctionId!).subscribe({
					next: () => Swal.fire("Deleted!", "The connector functions has been deleted.", "success").then(() => this.router.navigate(["/connectors"])),
					error: () => Swal.fire("Error", "An error has occurred while trying to delete the connector function.", "error")
				}).add(() => this.isLoading = false);
			}
		});
	}

	openLogs(connectorFunctionStderr: ConnectorFunctionStderrComponent): void {
		connectorFunctionStderr.open(this.connectorFunction?.buildStderr as string);
	}
}
