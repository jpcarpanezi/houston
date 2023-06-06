import { Component } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
	selector: 'app-pipeline-trigger',
	templateUrl: './pipeline-trigger.component.html',
	styleUrls: ['./pipeline-trigger.component.css']
})
export class PipelineTriggerComponent {
	public pipelineTriggerForm: FormGroup = this.initializePipelineTriggerForm();

	constructor(
		private fb: FormBuilder
	) { }

	private initializePipelineTriggerForm(): FormGroup {
		return this.pipelineTriggerForm = this.fb.group({
			sourceGit: ["", [
				Validators.required,
				Validators.maxLength(6000)
			]],
			secret: ["", [
				Validators.required,
				Validators.maxLength
			]],
			events: this.fb.array([])
		});
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
}
