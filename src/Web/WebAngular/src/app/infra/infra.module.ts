import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpInterceptorService } from './http/http-interceptor.service';
import { DurationPipe } from './helpers/duration.pipe';
import { UtcToLocalTimePipe } from './helpers/utc-to-local-time.pipe';



@NgModule({
	declarations: [
		DurationPipe,
 		UtcToLocalTimePipe
	],
	imports: [
		CommonModule
	],
	exports: [
		DurationPipe,
		UtcToLocalTimePipe
	],
	providers: [
		{ provide: HTTP_INTERCEPTORS, useClass: HttpInterceptorService, multi: true }
	]
})
export class InfraModule { }
