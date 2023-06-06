import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
	selector: 'app-pipeline-details',
	templateUrl: './pipeline-details.component.html',
	styleUrls: ['./pipeline-details.component.css']
})
export class PipelineDetailsComponent {
	public isLoading: boolean = false;
	public pipelineDetailsForm: FormGroup = this.initializePipelineDetailsForm();

	constructor(
		private fb: FormBuilder
	) { }

	private initializePipelineDetailsForm(): FormGroup {
		return this.pipelineDetailsForm = this.fb.group({
			name: ["", [
				Validators.required,
				Validators.maxLength(50)
			]],
			description: ["", [
				Validators.maxLength(5000)
			]]
		});
	}

	savePipelineDetails(): void {

	}
}
