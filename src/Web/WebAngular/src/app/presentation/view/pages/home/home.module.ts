import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';
import { SharedModule } from '../../shared/shared.module';
import { NewPipelineComponent } from './new-pipeline/new-pipeline.component';
import { UpdatePipelineComponent } from './update-pipeline/update-pipeline.component';


@NgModule({
	declarations: [
		HomeComponent,
		NewPipelineComponent,
  UpdatePipelineComponent
	],
	imports: [
		CommonModule,
		HomeRoutingModule,
		SharedModule
	]
})
export class HomeModule { }
