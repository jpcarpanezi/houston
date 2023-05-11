import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthUseCaseInterface } from './interfaces/use-cases/auth-use-case.interface';
import { AuthUseCaseService } from './use-cases/auth-use-case.service';



@NgModule({
	declarations: [],
	imports: [
		CommonModule
	],
	providers: [
		{ provide: AuthUseCaseInterface, useClass: AuthUseCaseService }
	]
})
export class DomainModule { }
