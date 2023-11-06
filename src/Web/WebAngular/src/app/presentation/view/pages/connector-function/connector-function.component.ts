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
import { ConnectorFunctionHistoryDetailViewModel } from 'src/app/domain/view-models/connector-function-history-detail.view-model';
import { ConnectorFunctionHistoryUseCaseInterface } from 'src/app/domain/interfaces/use-cases/connector-function-history-use-case.interface';
import { HttpErrorResponse } from '@angular/common/http';

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

	public connectorFunctionHistory?: ConnectorFunctionHistoryDetailViewModel;
	public connectorFunctionId?: string;
	public connectorFunctionHistoryId?: string | null;

	public buildStatus?: BuildStatus;
	public longPooling?: Subscription;

	constructor(
		private fb: FormBuilder,
		private route: ActivatedRoute,
		private router: Router,
		private connectorFunctionHistoryUseCase: ConnectorFunctionHistoryUseCaseInterface
	) { }

	ngOnInit(): void {
		this.connectorFunctionId = this.route.snapshot.queryParams["connectorFunctionId"];
		this.connectorFunctionHistoryId = this.route.snapshot.paramMap.get("id");

		if (!this.connectorFunctionId && !this.connectorFunctionHistoryId) {
			this.router.navigate(["/connectors"]);
		}

		if (this.connectorFunctionHistoryId) {
			this.getConnectorFunction(this.connectorFunctionHistoryId);
			this.triggerLongPooling();
		}
	}

	ngOnDestroy(): void {
		this.longPooling?.unsubscribe();
	}

	private triggerLongPooling(): void {
		this.longPooling = interval(5000).pipe(
			startWith(0),
			switchMap(() => this.connectorFunctionHistoryUseCase.get(this.connectorFunctionHistoryId!)),
			takeWhile(x => x.buildStatus !== "Success" && x.buildStatus !== "Failed", true)
		).subscribe({
			next: (response: ConnectorFunctionHistoryDetailViewModel) => {
				this.buildStatus = response.buildStatus;
				this.connectorFunctionHistory = response;
			},
			error: () => Swal.fire("Error", "An error has occurred while trying to get the connector function.", "error")
		});
	}

	private initializeConnectorFunctionForm(): FormGroup {
		return this.connectorFunctionForm = this.fb.group({
			version: ["", [
				Validators.required,
				Validators.pattern(/^\d+\.\d+\.\d+$/)
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

		this.connectorFunctionHistoryUseCase.get(connectorFunctionId).subscribe({
			next: (response: ConnectorFunctionHistoryDetailViewModel) => this.getConnectorFunctionNext(response),
			error: () => Swal.fire("Error", "An error has occurred while getting the connector function.", "error")
		}).add(() => this.isLoading = false);
	}

	private getConnectorFunctionNext(response: ConnectorFunctionHistoryDetailViewModel): void {
		this.connectorFunctionHistory = response;
		this.buildStatus = response.buildStatus;

		response.inputs?.forEach((input: ConnectorFunctionInputViewModel) => {
			this.addInput(input.id);

			input.values?.forEach(() => {
				this.addInputValue(this.inputs.controls[this.inputs.controls.length - 1]);
			});
		});

		this.connectorFunctionForm.patchValue(response);
		this.codeEditor?.codeMirror?.getDoc().setValue(window.atob(response.script as string));
		this.packageEditor?.codeMirror?.getDoc().setValue(window.atob(response.package as string));
	}

	addInput(inputId: string | null): void {
		const inputFormGroup = this.fb.group({
			id: [inputId, [
				Validators.maxLength(36)
			]],
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
		if (this.connectorFunctionHistoryId) {
			Swal.fire({
				icon: "question",
				title: "Are you sure?",
				text: "Updating a connector may cause unexpected behavior in active pipelines.",
				showDenyButton: true,
				showConfirmButton: true,
				confirmButtonText: "Yes, save it",
				denyButtonText: "No, cancel",
			}).then((result) => {
				if (result.isConfirmed) {
					this.saveConnectorFunctionNext();
				}
			});
		} else {
			this.saveConnectorFunctionNext();
		}
	}

	private saveConnectorFunctionNext(): void {
		if (this.connectorFunctionForm.invalid) return;
		this.connectorFunctionForm.disable();

		const script: string | undefined = window.btoa(this.codeEditor?.codeMirror?.getDoc().getValue()!);
		const packageJson: string | undefined = window.btoa(this.packageEditor?.codeMirror?.getDoc().getValue()!);
		this.connectorFunctionForm.value["connectorFunctionId"] = this.connectorFunctionId;
		this.connectorFunctionForm.value["script"] = script;
		this.connectorFunctionForm.value["package"] = packageJson;

		if (this.connectorFunctionHistory) {
			this.connectorFunctionForm.value["id"] = this.connectorFunctionHistory.id;

			this.connectorFunctionHistoryUseCase.update(this.connectorFunctionForm.value).subscribe({
				next: (response: ConnectorFunctionHistoryDetailViewModel) => {
					Toast.fire({
						icon: "success",
						title: "Updated successfully."
					});

					this.buildStatus = response.buildStatus;
					this.connectorFunctionHistory = response;
					this.triggerLongPooling();
				},
				error: () => Swal.fire("Error", "An error has occurred while updating the connector function.", "error")
			}).add(() => this.connectorFunctionForm.enable());
		} else {
			this.connectorFunctionHistoryUseCase.create(this.connectorFunctionForm.value).subscribe({
				next: (response: ConnectorFunctionHistoryDetailViewModel) => this.router.navigate(["/connector-function", response.id]),
				error: (error: HttpErrorResponse[]) => {
					if (error[0].error["errorCode"] == "versionAlreadyExists") {
						Swal.fire("Error", "A connector function with this version already exists.", "error");
					} else {
						Swal.fire("Error", "An error has occurred while creating the connector function.", "error");
					}
				}
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

				this.connectorFunctionHistoryUseCase.delete(this.connectorFunctionHistoryId!).subscribe({
					next: () => Swal.fire("Deleted!", "The connector functions has been deleted.", "success").then(() => this.router.navigate(["/connectors"])),
					error: () => Swal.fire("Error", "An error has occurred while trying to delete the connector function.", "error")
				}).add(() => this.isLoading = false);
			}
		});
	}

	openLogs(connectorFunctionStderr: ConnectorFunctionStderrComponent): void {
		connectorFunctionStderr.open(this.connectorFunctionHistory?.buildStderr as string);
	}
}
