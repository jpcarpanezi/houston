import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthUseCaseInterface } from './interfaces/use-cases/auth-use-case.interface';
import { AuthUseCaseService } from './use-cases/auth-use-case.service';
import { UserUseCaseInterface } from './interfaces/use-cases/user-use-case.interface';
import { UserUseCaseService } from './use-cases/user-use-case.service';



@NgModule({
	declarations: [],
	imports: [
		CommonModule
	],
	providers: [
		{ provide: AuthUseCaseInterface, useClass: AuthUseCaseService },
		{ provide: UserUseCaseInterface, useClass: UserUseCaseService }
	]
})
export class DomainModule { }
