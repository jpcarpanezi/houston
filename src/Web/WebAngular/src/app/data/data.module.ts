import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthRepositoryInterface } from '../domain/interfaces/repositories/auth-repository.interface';
import { AuthRepositoryService } from './repositories/auth-repository.service';
import { HttpClientModule } from '@angular/common/http';



@NgModule({
	declarations: [],
	imports: [
		CommonModule,
		HttpClientModule
	],
	providers: [
		{ provide: AuthRepositoryInterface, useClass: AuthRepositoryService }
	]
})
export class DataModule { }
