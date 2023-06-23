import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpInterceptorService } from './http/http-interceptor.service';
import { DurationPipe } from './helpers/duration.pipe';



@NgModule({
	declarations: [
		DurationPipe
	],
	imports: [
		CommonModule
	],
	exports: [
		DurationPipe
	],
	providers: [
		{ provide: HTTP_INTERCEPTORS, useClass: HttpInterceptorService, multi: true }
	]
})
export class InfraModule { }
