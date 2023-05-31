import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PipelineRoutingModule } from './pipeline-routing.module';
import { PipelineComponent } from './pipeline.component';


@NgModule({
	declarations: [
		PipelineComponent
	],
	imports: [
		CommonModule,
		PipelineRoutingModule
	]
})
export class PipelineModule { }
