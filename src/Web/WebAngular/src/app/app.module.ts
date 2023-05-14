import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { DomainModule } from './domain/domain.module';
import { DataModule } from './data/data.module';
import { InfraModule } from './infra/infra.module';
import { PresentationModule } from './presentation/presentation.module';
import { FaConfig, FaIconLibrary, FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faCircleCheck, faCircleXmark, faRightToBracket } from '@fortawesome/free-solid-svg-icons';
import { CookieService } from 'ngx-cookie-service';

@NgModule({
	declarations: [
		AppComponent
	],
	imports: [
		BrowserModule,
		DomainModule,
		DataModule,
		InfraModule,
		PresentationModule,
  		FontAwesomeModule
	],
	providers: [CookieService],
	bootstrap: [AppComponent]
})
export class AppModule {
	constructor(library: FaIconLibrary, faConfig: FaConfig) {
		faConfig.fixedWidth = true;
		library.addIcons(faRightToBracket, faCircleCheck, faCircleXmark)
	}
}
