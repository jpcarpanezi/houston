import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConnectorsRoutingModule } from './connectors-routing.module';
import { ConnectorsComponent } from './connectors.component';
import { SharedModule } from '../../shared/shared.module';
import { NewConnectorComponent } from './new-connector/new-connector.component';
import { UpdateConnectorComponent } from './update-connector/update-connector.component';
import { NewConnectorFunctionComponent } from './new-connector-function/new-connector-function.component';
import { UpdateConnectorFunctionComponent } from './update-connector-function/update-connector-function.component';


@NgModule({
	declarations: [
		ConnectorsComponent,
		NewConnectorComponent,
		UpdateConnectorComponent,
		NewConnectorFunctionComponent,
		UpdateConnectorFunctionComponent
	],
	imports: [
		CommonModule,
		ConnectorsRoutingModule,
		SharedModule
	]
})
export class ConnectorsModule { }
