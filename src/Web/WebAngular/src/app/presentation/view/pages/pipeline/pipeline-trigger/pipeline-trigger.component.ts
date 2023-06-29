import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { PipelineTriggerUseCaseInterface } from 'src/app/domain/interfaces/use-cases/pipeline-trigger-use-case.interface';
import { PipelineTriggerViewModel } from 'src/app/domain/view-models/pipeline-trigger.view-model';
import { Toast } from 'src/app/infra/helpers/toast';
import Swal from 'sweetalert2';

@Component({
	selector: 'app-pipeline-trigger',
	templateUrl: './pipeline-trigger.component.html',
	styleUrls: ['./pipeline-trigger.component.css']
})
export class PipelineTriggerComponent implements OnInit {
	public isLoading: boolean = true;
	public pipelineTriggerForm: FormGroup = this.initializePipelineTriggerForm();
	public pipelineId: string | null = null;
	public pipelineTrigger?: PipelineTriggerViewModel;
	public originPath = location.origin;

	constructor(
		private fb: FormBuilder,
		private route: ActivatedRoute,
		private pipelineTriggerUseCase: PipelineTriggerUseCaseInterface
	) { }

	ngOnInit(): void {
		this.pipelineId = this.route.snapshot.paramMap.get("id");

		if (this.pipelineId)
			this.getPipelineTrigger(this.pipelineId);
	}

	private initializePipelineTriggerForm(): FormGroup {
		return this.pipelineTriggerForm = this.fb.group({
			sourceGit: ["", [
				Validators.required,
				Validators.pattern('^git@github\\.com:.+\\.git$'),
				Validators.maxLength(6000)
			]],
			secret: ["", []],
			events: this.fb.array([])
		});
	}

	private getPipelineTrigger(pipelineId: string): void {
		this.pipelineTriggerUseCase.get(pipelineId).subscribe({
			next: (response: PipelineTriggerViewModel) => this.getPipelineTriggerNext(response),
			error: (error: HttpErrorResponse[]) => this.getPipelineTriggerError(error[0])
		}).add(() => this.isLoading = false);
	}

	private getPipelineTriggerNext(response: PipelineTriggerViewModel): void {
		Object.assign(response, { events: response.pipelineTriggerEvents })["pipelineTriggerEvents"];

		for (let i = 0; i < response.pipelineTriggerEvents.length; i++) {
			const event = response.pipelineTriggerEvents[i];
			this.addEvent();
			Object.assign(event, { eventFilters: event.pipelineTriggerFilters })["pipelineTriggerFilters"];

			for (let j = 0; j < event.pipelineTriggerFilters.length; j++) {
				const filter = event.pipelineTriggerFilters[j];
				this.addEventFilter(this.getEvents().controls[i]);

				for (let k = 0; k < filter.filterValues.length - 1; k++) {
					this.addEventFilterValue(this.getEventControl(this.getEvents().controls[i], "eventFilters").controls[j]);
				}
			}
		}

		this.pipelineTriggerForm.get("secret")?.disable();
		this.pipelineTriggerForm.patchValue(response);
		this.pipelineTrigger = response;
	}

	private getPipelineTriggerError(error: HttpErrorResponse): void {
		if (error.status != HttpStatusCode.NotFound) {
			Swal.fire("Error", "An error has occurred while trying to get the pipeline trigger. Please try again later.", "error");
		} else {
			this.togglePasswordChange(true);
		}
	}

	savePipelineTrigger(): void {
		if (this.pipelineTriggerForm.invalid) return;
		this.isLoading = true;

		if (!this.pipelineTrigger) {
			this.createPipelineTrigger();
		} else {
			this.updatePipelineTrigger();
			this.changePipelineSecret();
		}
	}

	private createPipelineTrigger(): void {
		this.pipelineTriggerForm.value["pipelineId"] = this.pipelineId;
		this.pipelineTriggerUseCase.create(this.pipelineTriggerForm.value).subscribe({
			next: (response: PipelineTriggerViewModel) => {
				 Toast.fire({ icon: "success", title: "Saved successfully" });
				 this.pipelineTrigger = response;
			},
			error: () => Swal.fire("Error", "An error has occurred while trying to create the pipeline trigger. Please try again later.", "error")
		}).add(() => this.isLoading = false);
	}

	private updatePipelineTrigger(): void {
		this.pipelineTriggerForm.value["pipelineTriggerId"] = this.pipelineTrigger?.id;
		this.pipelineTriggerUseCase.update(this.pipelineTriggerForm.value).subscribe({
			next: () => Toast.fire({ icon: "success", title: "Saved successfully" }),
			error: () => Swal.fire("Error", "An error has occurred while trying to update the pipeline trigger. Please try again later.", "error")
		}).add(() => this.isLoading = false);
	}

	private changePipelineSecret(): void {
		if (!this.pipelineTriggerForm.get("secret")?.enabled) {
			return;
		}

		this.pipelineTriggerForm.value["pipelineTriggerId"] = this.pipelineTrigger?.id;
		this.pipelineTriggerForm.value["newSecret"] = this.pipelineTriggerForm.value["secret"];
		this.pipelineTriggerUseCase.changeSecret(this.pipelineTriggerForm.value).subscribe({
			next: () => Toast.fire({ icon: "success", title: "Secret changed successfully" }),
			error: () => Swal.fire("Error", "An error has occurred while trying to change the pipeline secret. Please try again later.", "error")
		}).add(() => this.isLoading = false);
	}

	getEvents(): FormArray {
		return this.pipelineTriggerForm.get("events") as FormArray;
	}

	getEventControl(control: AbstractControl, path: string): FormArray {
		return control.get(path) as FormArray;
	}

	addEvent(): void {
		const event = this.fb.group({
			triggerEventId: ["", [
				Validators.required
			]],
			eventFilters: this.fb.array([])
		});

		this.getEvents().push(event);
	}

	addEventFilter(control: AbstractControl): void {
		const eventFilter = this.fb.group({
			triggerFilterId: ["", [
				Validators.required
			]],
			filterValues: this.fb.array([
				this.fb.control("", [
					Validators.required
				])
			])
		});

		this.getEventControl(control, "eventFilters").push(eventFilter);
	}

	addEventFilterValue(control: AbstractControl): void {
		const eventFilterValue = this.fb.control("", [
			Validators.required
		]);

		this.getEventControl(control, "filterValues").push(eventFilterValue);
	}

	removeEvent(index: number): void {
		this.getEvents().removeAt(index);
	}

	removeEventFilter(control: AbstractControl, index: number): void {
		this.getEventControl(control, "eventFilters").removeAt(index);
	}

	removeEventFilterValue(control: AbstractControl, index: number): void {
		this.getEventControl(control, "filterValues").removeAt(index);
	}

	togglePasswordChange(forceEnabled: boolean = false): void {
		const secretControl = this.pipelineTriggerForm.get("secret");

		forceEnabled || secretControl?.disabled ? secretControl?.enable() : secretControl?.disable();

		if (!secretControl?.disabled) {
			secretControl?.addValidators([
				Validators.required,
				Validators.minLength(8),
				Validators.maxLength(64),
				Validators.pattern(/\d/),
				Validators.pattern(/[A-Z]/)
			]);
		} else {
			secretControl?.clearValidators();
		}

		secretControl?.updateValueAndValidity();
	}

	updateDeployKeys(): void {
		this.isLoading = true;

		this.pipelineTriggerUseCase.updateDeployKeys(this.pipelineId as string).subscribe({
			next: () => this.pipelineTrigger!.keyRevealed = false,
			error: () => Swal.fire("Error", "An error has occurred while trying to update the deploy keys. Please try again later.", "error")
		}).add(() => this.isLoading = false);
	}

	copyToClipboard(input: HTMLInputElement, button: HTMLButtonElement): void {
		navigator.clipboard.writeText(input.value).then(() => {
			const defaultValue = button.innerHTML;
			button.innerHTML = "Copied!";

			setTimeout(() => {
				button.innerHTML = defaultValue;
			}, 500);
		});
	}
}
