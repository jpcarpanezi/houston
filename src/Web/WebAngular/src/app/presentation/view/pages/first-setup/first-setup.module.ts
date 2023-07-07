import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FirstSetupRoutingModule } from './first-setup-routing.module';
import { FirstSetupComponent } from './first-setup.component';
import { SharedModule } from '../../shared/shared.module';


@NgModule({
	declarations: [
		FirstSetupComponent
	],
	imports: [
		CommonModule,
		FirstSetupRoutingModule,
		SharedModule
	]
})
export class FirstSetupModule { }
