import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthUseCaseInterface } from './interfaces/use-cases/auth-use-case.interface';
import { AuthUseCaseService } from './use-cases/auth-use-case.service';
import { UserUseCaseInterface } from './interfaces/use-cases/user-use-case.interface';
import { UserUseCaseService } from './use-cases/user-use-case.service';
import { ConnectorUseCaseInterface } from './interfaces/use-cases/connector-use-case.interface';
import { ConnectorUseCaseService } from './use-cases/connector-use-case.service';
import { ConnectorFunctionUseCaseInterface } from './interfaces/use-cases/connector-function-use-case.interface';
import { ConnectorFunctionUseCaseService } from './use-cases/connector-function-use-case.service';



@NgModule({
	declarations: [],
	imports: [
		CommonModule
	],
	providers: [
		{ provide: AuthUseCaseInterface, useClass: AuthUseCaseService },
		{ provide: UserUseCaseInterface, useClass: UserUseCaseService },
		{ provide: ConnectorUseCaseInterface, useClass: ConnectorUseCaseService },
		{ provide: ConnectorFunctionUseCaseInterface, useClass: ConnectorFunctionUseCaseService }
	]
})
export class DomainModule { }
