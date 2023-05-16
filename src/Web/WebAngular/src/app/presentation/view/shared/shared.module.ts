import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { FormErrorsComponent } from './form-errors/form-errors.component';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { NgxLoaderIndicatorDirective, provideNgxLoaderIndicator } from 'ngx-loader-indicator';
import { provideAnimations } from '@angular/platform-browser/animations';



@NgModule({
	declarations: [
		FormErrorsComponent
	],
	imports: [
		CommonModule,
		NgxLoaderIndicatorDirective
	],
	exports: [
		ReactiveFormsModule,
		FontAwesomeModule,
		SweetAlert2Module,
		FormErrorsComponent,
		NgxLoaderIndicatorDirective
	],
	providers: [
		provideAnimations(),
		provideNgxLoaderIndicator()
	]
})
export class SharedModule { }
