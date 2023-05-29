import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConnectorsRoutingModule } from './connectors-routing.module';
import { ConnectorsComponent } from './connectors.component';
import { SharedModule } from '../../shared/shared.module';
import { NewConnectorComponent } from './new-connector/new-connector.component';
import { UpdateConnectorComponent } from './update-connector/update-connector.component';


@NgModule({
	declarations: [
		ConnectorsComponent,
  		NewConnectorComponent,
    	UpdateConnectorComponent
	],
	imports: [
		CommonModule,
		ConnectorsRoutingModule,
		SharedModule
	]
})
export class ConnectorsModule { }
