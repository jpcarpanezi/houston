import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { FormErrorsComponent } from './form-errors/form-errors.component';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';



@NgModule({
	declarations: [
		FormErrorsComponent
	],
	imports: [
		CommonModule
	],
	exports: [
		ReactiveFormsModule,
		FontAwesomeModule,
		SweetAlert2Module,
		FormErrorsComponent
	]
})
export class SharedModule { }
