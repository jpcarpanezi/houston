import { Component, ViewChild } from '@angular/core';
import { PipelineDetailsComponent } from './pipeline-details/pipeline-details.component';

@Component({
	selector: 'app-pipeline',
	templateUrl: './pipeline.component.html',
	styleUrls: ['./pipeline.component.css']
})
export class PipelineComponent {
	@ViewChild("pipelineDetails") public pipelineDetails?: PipelineDetailsComponent;

	constructor() { }

	savePipeline(): void {
		this.pipelineDetails?.pipelineDetailsForm.markAllAsTouched();

		if (this.pipelineDetails?.pipelineDetailsForm.invalid) return;

		this.pipelineDetails?.savePipelineDetails();
	}
}
