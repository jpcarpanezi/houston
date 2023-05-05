import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { DomainModule } from './domain/domain.module';
import { DataModule } from './data/data.module';
import { InfraModule } from './infra/infra.module';
import { PresentationModule } from './presentation/presentation.module';

@NgModule({
	declarations: [
		AppComponent
	],
	imports: [
		BrowserModule,
		DomainModule,
		DataModule,
		InfraModule,
		PresentationModule
	],
	providers: [],
	bootstrap: [AppComponent]
})
export class AppModule { }
