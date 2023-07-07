import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RunsRoutingModule } from './runs-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { RunsComponent } from './runs.component';
import { InfraModule } from 'src/app/infra/infra.module';


@NgModule({
	declarations: [
		RunsComponent
	],
	imports: [
		CommonModule,
		RunsRoutingModule,
		SharedModule,
		InfraModule
	]
})
export class RunsModule { }
