import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RunRoutingModule } from './run-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { RunComponent } from './run.component';
import { CodemirrorModule } from '@ctrl/ngx-codemirror';
import { InfraModule } from 'src/app/infra/infra.module';


@NgModule({
	declarations: [
		RunComponent
	],
	imports: [
		CommonModule,
		RunRoutingModule,
		SharedModule,
		CodemirrorModule,
		InfraModule
	]
})
export class RunModule { }
