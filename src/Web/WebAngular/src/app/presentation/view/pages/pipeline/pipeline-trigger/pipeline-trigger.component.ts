import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { PipelineTriggerUseCaseInterface } from 'src/app/domain/interfaces/use-cases/pipeline-trigger-use-case.interface';

@Component({
	selector: 'app-pipeline-trigger',
	templateUrl: './pipeline-trigger.component.html',
	styleUrls: ['./pipeline-trigger.component.css']
})
export class PipelineTriggerComponent implements OnInit {
	public isLoading: boolean = true;
	public pipelineTriggerForm: FormGroup = this.initializePipelineTriggerForm();
	public pipelineId: string | null = null;
	public pipelineTriggerId: string | null = null;

	constructor(
		private fb: FormBuilder,
		private route: ActivatedRoute,
		private pipelineTriggerUseCase: PipelineTriggerUseCaseInterface
	) { }

	ngOnInit(): void {
		this.pipelineId = this.route.snapshot.paramMap.get("id");
	}

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
