import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthRepositoryInterface } from '../domain/interfaces/repositories/auth-repository.interface';
import { AuthRepositoryService } from './repositories/auth-repository.service';
import { HttpClientModule } from '@angular/common/http';
import { UserRepositoryInterface } from '../domain/interfaces/repositories/user-repository.interface';
import { UserRepositoryService } from './repositories/user-repository.service';
import { ConnectorRepositoryInterface } from '../domain/interfaces/repositories/connector-repository.interface';
import { ConnectorRepositoryService } from './repositories/connector-repository.service';
import { ConnectorFunctionRepositoryInterface } from '../domain/interfaces/repositories/connector-function-repository.interface';
import { ConnectorFunctionRepositoryService } from './repositories/connector-function-repository.service';



@NgModule({
	declarations: [],
	imports: [
		CommonModule,
		HttpClientModule
	],
	providers: [
		{ provide: AuthRepositoryInterface, useClass: AuthRepositoryService },
		{ provide: UserRepositoryInterface, useClass: UserRepositoryService },
		{ provide: ConnectorRepositoryInterface, useClass: ConnectorRepositoryService },
		{ provide: ConnectorFunctionRepositoryInterface, useClass: ConnectorFunctionRepositoryService }
	]
})
export class DataModule { }
