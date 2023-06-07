import { Component, ViewChild } from '@angular/core';
import { PipelineDetailsComponent } from './pipeline-details/pipeline-details.component';
import { PipelineTriggerComponent } from './pipeline-trigger/pipeline-trigger.component';

@Component({
	selector: 'app-pipeline',
	templateUrl: './pipeline.component.html',
	styleUrls: ['./pipeline.component.css']
})
export class PipelineComponent {
	@ViewChild("pipelineDetails") public pipelineDetails?: PipelineDetailsComponent;
	@ViewChild("pipelineTrigger") public pipelineTrigger?: PipelineTriggerComponent;

	constructor() { }

	savePipeline(): void {
		this.pipelineDetails?.pipelineDetailsForm.markAllAsTouched();
		this.pipelineTrigger?.pipelineTriggerForm.markAllAsTouched();

		if (this.pipelineDetails?.pipelineDetailsForm.invalid) return;
		if (this.pipelineTrigger?.pipelineTriggerForm.invalid) return;

		this.pipelineDetails?.savePipelineDetails();
		this.pipelineTrigger?.savePipelineTrigger();
	}
}
