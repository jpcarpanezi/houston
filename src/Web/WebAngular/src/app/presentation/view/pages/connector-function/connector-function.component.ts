import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CodemirrorComponent } from '@ctrl/ngx-codemirror';
import { Subscription, interval, startWith, switchMap, takeWhile } from 'rxjs';
import { BuildStatus } from 'src/app/domain/enums/build-status.enum';
import { ConnectorFunctionUseCaseInterface } from 'src/app/domain/interfaces/use-cases/connector-function-use-case.interface';
import { Toast } from 'src/app/infra/helpers/toast';
import Swal from 'sweetalert2';
import { ConnectorFunctionStderrComponent } from './connector-function-stderr/connector-function-stderr.component';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { parse, stringify } from 'yaml';
import { ConnectorFunctionDetailViewModel } from 'src/app/domain/view-models/connector-function-detail.view-model';
import { CreateConnectorFunctionCommand } from 'src/app/domain/commands/connector-function-commands/create-connector-function.command';
import { UpdateConnectorFunctionCommand } from 'src/app/domain/commands/connector-function-commands/update-connector-function.command';
import { InputType } from 'src/app/domain/enums/input-type.enum';
import { SpecFileViewInputsViewModel, SpecFileViewModel } from 'src/app/domain/view-models/spec-file.view-model';
import { CaseConverterHelper } from 'src/app/infra/helpers/case-converter.helper';


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
	public yamlFile: string = "";

	public connector?: string;
	public function?: string;
	public buildStderr: string | null = null;
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
		this.connector = this.route.snapshot.queryParams["connector"];
		this.connectorFunctionForm.get("connector")?.setValue(this.connector);

		this.function = this.route.snapshot.queryParams["function"];
		this.connectorFunctionForm.get("function")?.setValue(this.function);
		this.connectorFunctionId = this.route.snapshot.paramMap.get("id");

		this.connectorFunctionForm.valueChanges.subscribe(value => this.convertToYaml(value));

		if (!this.connector) {
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
			next: (response: ConnectorFunctionDetailViewModel) => {
				this.buildStatus = response.buildStatus;
				this.buildStderr = response.buildStderr;
			},
			error: () => Swal.fire("Error", "An error has occurred while trying to get the connector function.", "error")
		});
	}

	private initializeConnectorFunctionForm(): FormGroup {
		return this.connectorFunctionForm = this.fb.group({
			friendlyName: ["", [
				Validators.required,
				Validators.maxLength(50)
			]],
			function: ["", [
				Validators.required,
				Validators.maxLength(64),
				Validators.pattern("^[a-z][a-z0-9-]*$")
			]],
			version: ["", [
				Validators.required,
				Validators.pattern(/^\d+\.\d+\.\d+$/)
			]],
			connector: ["", [
				Validators.required,
				Validators.maxLength(64),
				Validators.pattern("^[a-z][a-z0-9-]*$")
			]],
			using: ["node20", [
				Validators.required
			]],
			description: ["", [
				Validators.maxLength(5000)
			]],
			inputs: this.fb.array([])
		});
	}

	private convertToYaml(value: any): void {
		const yamlObject = { ...value };

		yamlObject.runs = {
			using: yamlObject.using
		};
		delete yamlObject.using;

		const inputs: any = {};
		yamlObject.inputs.forEach((input: any) => {
			inputs[input.replace] = {
				type: input.type,
				label: input.label,
				placeholder: input.placeholder,
				values: input.values,
				default: input.default,
				required: input.required,
				advanced: input.advanced
			}

			if (inputs[input.replace].default === "") {
				inputs[input.replace].default = null;
			}

			if (inputs[input.replace].values.length === 0) {
				inputs[input.replace].values = null;
			}
		});

		if (Object.keys(inputs).length === 0) {
			delete yamlObject.inputs;
		} else {
			yamlObject.inputs = inputs;
		}

		const sanitizedObject = CaseConverterHelper.camelToSnake(yamlObject);

		this.yamlFile = stringify(sanitizedObject);
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
			next: (response: ConnectorFunctionDetailViewModel) => this.getConnectorFunctionNext(response),
			error: () => Swal.fire("Error", "An error has occurred while getting the connector function.", "error")
		}).add(() => this.isLoading = false);
	}

	private getConnectorFunctionNext(response: ConnectorFunctionDetailViewModel): void {
		this.yamlFile = window.atob(response.specsFile as string);
		this.buildStatus = response.buildStatus;
		this.buildStderr = response.buildStderr;

		const parsedYaml: SpecFileViewModel = CaseConverterHelper.snakeToCamel(parse(this.yamlFile));

		if (parsedYaml.inputs) {
			const resultArray: SpecFileViewInputsViewModel[] = [];

			Object.keys(parsedYaml.inputs).forEach((key: string) => {
				this.addInput();

				const value = (parsedYaml.inputs as Record<string, SpecFileViewInputsViewModel>)[key];

				if (Array.isArray(value.values)) {
					value.values.forEach(() => this.addInputValue(this.inputs.controls[this.inputs.controls.length - 1]));
				}

				const newObj: SpecFileViewInputsViewModel = { ...value, replace: key };

				resultArray.push(newObj);
			});

			parsedYaml.inputs = resultArray;
		}

		this.connectorFunctionForm.patchValue(parsedYaml);
		this.codeEditor?.codeMirror?.getDoc().setValue(window.atob(response.script as string));
		this.packageEditor?.codeMirror?.getDoc().setValue(window.atob(response.package as string));
	}

	addInput(): void {
		const inputFormGroup = this.fb.group({
			type: ["", [
				Validators.required
			]],
			label: ["", [
				Validators.required,
				Validators.maxLength(25)
			]],
			placeholder: ["", [
				Validators.required,
				Validators.maxLength(50)
			]],
			replace: ["", [
				Validators.required,
				Validators.maxLength(25),
				Validators.pattern("^[a-z][a-z0-9-]*$")
			]],
			values: this.fb.array([]),
			default: [null, [
				Validators.maxLength(100)
			]],
			required: [false],
			advanced: [false]
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
		this.connectorFunctionForm.markAllAsTouched();

		if (this.connectorFunctionId) {
			Swal.fire({
				icon: "question",
				title: "Are you sure?",
				text: "Updating a function may cause unexpected behavior in active pipelines.",
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

		const specFile: File = this.createFile(this.yamlFile, "application/x-yaml", "spec.yaml");
		const scriptFile: File = this.createFile(this.codeEditor?.codeMirror?.getDoc().getValue()!, "text/javascript", "script.js");
		const packageFile: File = this.createFile(this.packageEditor?.codeMirror?.getDoc().getValue()!, "application/json", "package.json");

		const body: CreateConnectorFunctionCommand = {
			specFile: specFile,
			script: scriptFile,
			package: packageFile
		}

		const updateBody: UpdateConnectorFunctionCommand = {
			...body,
			id: this.connectorFunctionId!
		}

		if (this.connectorFunctionId) {
			this.connectorFunctionForm.value["id"] = this.connectorFunctionId;

			this.connectorFunctionUseCase.update(updateBody).subscribe({
				next: (response: ConnectorFunctionDetailViewModel) => {
					Toast.fire({
						icon: "success",
						title: "Updated successfully."
					});

					this.buildStatus = response.buildStatus;
					this.triggerLongPooling();
				},
				error: () => Swal.fire("Error", "An error has occurred while updating the connector function.", "error")
			}).add(() => this.connectorFunctionForm.enable());
		} else {
			this.connectorFunctionUseCase.create(body).subscribe({
				next: (response: ConnectorFunctionDetailViewModel) => this.router.navigate(["/connector-function", response.id], { queryParams: { connector: this.connector } }),
				error: (error: HttpErrorResponse[]) => {
					if (error[0].status == HttpStatusCode.Conflict) {
						Swal.fire("Error", "A connector function with this version already exists.", "error");
					} else {
						Swal.fire("Error", "An error has occurred while creating the connector function.", "error");
					}
				}
			}).add(() => this.connectorFunctionForm.enable());
		}
	}

	private createFile(content: string, type: string, fileName: string): File {
		const blob: Blob = new Blob([content], { type: type });
		const file: File = new File([blob], fileName);

		return file;
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
		connectorFunctionStderr.open(this.buildStderr as string);
	}
}
