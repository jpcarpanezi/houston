import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConnectorsRoutingModule } from './connectors-routing.module';
import { ConnectorsComponent } from './connectors.component';
import { SharedModule } from '../../shared/shared.module';


@NgModule({
	declarations: [
		ConnectorsComponent
	],
	imports: [
		CommonModule,
		ConnectorsRoutingModule,
		SharedModule
	]
})
export class ConnectorsModule { }
