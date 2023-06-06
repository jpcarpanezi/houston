import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PipelineRoutingModule } from './pipeline-routing.module';
import { PipelineComponent } from './pipeline.component';
import { SharedModule } from '../../shared/shared.module';
import { PipelineDetailsComponent } from './pipeline-details/pipeline-details.component';
import { PipelineTriggerComponent } from './pipeline-trigger/pipeline-trigger.component';
import { PipelineInstructionsComponent } from './pipeline-instructions/pipeline-instructions.component';


@NgModule({
	declarations: [
		PipelineComponent,
		PipelineDetailsComponent,
		PipelineTriggerComponent,
		PipelineInstructionsComponent
	],
	imports: [
		CommonModule,
		PipelineRoutingModule,
		SharedModule
	]
})
export class PipelineModule { }
