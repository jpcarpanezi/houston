import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FirstAccessRoutingModule } from './first-access-routing.module';
import { FirstAccessComponent } from './first-access.component';
import { SharedModule } from '../../shared/shared.module';


@NgModule({
	declarations: [
    FirstAccessComponent
  ],
	imports: [
		CommonModule,
		FirstAccessRoutingModule,
		SharedModule
	]
})
export class FirstAccessModule { }
