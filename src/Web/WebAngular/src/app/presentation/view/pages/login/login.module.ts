import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LoginRoutingModule } from './login-routing.module';
import { LoginComponent } from './login.component';
import { SharedModule } from '../../shared/shared.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


@NgModule({
	declarations: [
		LoginComponent
	],
	imports: [
		CommonModule,
		LoginRoutingModule,
		BrowserAnimationsModule,
		SharedModule
	]
})
export class LoginModule { }
