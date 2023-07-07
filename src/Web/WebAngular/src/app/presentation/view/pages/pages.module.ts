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
import { RunsModule } from './runs/runs.module';
import { RunComponent } from './run/run.component';
import { RunModule } from './run/run.module';
import { UsersModule } from './users/users.module';



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
		RunsModule,
		RunModule,
		UsersModule,
		PagesRoutingModule
	],
	exports: [PagesRoutingModule]
})
export class PagesModule { }
