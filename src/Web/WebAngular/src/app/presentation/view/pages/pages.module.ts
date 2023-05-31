import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PagesRoutingModule } from './pages-routing.module';
import { LoginModule } from './login/login.module';
import { HomeModule } from './home/home.module';
import { SharedModule } from '../shared/shared.module';
import { FirstAccessModule } from './first-access/first-access.module';
import { FirstSetupModule } from './first-setup/first-setup.module';
import { ConnectorsModule } from './connectors/connectors.module';
import { ConnectorFunctionModule } from './connector-function/connector-function.module';
import { PipelineModule } from './pipeline/pipeline.module';



@NgModule({
	declarations: [],
	imports: [
		CommonModule,
		LoginModule,
		FirstSetupModule,
		HomeModule,
		ConnectorsModule,
		ConnectorFunctionModule,
		PipelineModule,
		FirstAccessModule,
		SharedModule,
		PagesRoutingModule
	],
	exports: [PagesRoutingModule]
})
export class PagesModule { }
