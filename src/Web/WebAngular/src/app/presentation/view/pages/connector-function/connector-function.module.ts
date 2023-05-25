import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConnectorFunctionRoutingModule } from './connector-function-routing.module';
import { ConnectorFunctionComponent } from './connector-function.component';
import { SharedModule } from '../../shared/shared.module';
import { CodemirrorModule } from '@ctrl/ngx-codemirror';


@NgModule({
	declarations: [
		ConnectorFunctionComponent
	],
	imports: [
		CommonModule,
		ConnectorFunctionRoutingModule,
		SharedModule,
		CodemirrorModule
	]
})
export class ConnectorFunctionModule { }
