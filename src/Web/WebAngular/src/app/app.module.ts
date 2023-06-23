import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { DomainModule } from './domain/domain.module';
import { DataModule } from './data/data.module';
import { InfraModule } from './infra/infra.module';
import { PresentationModule } from './presentation/presentation.module';
import { FaConfig, FaIconLibrary, FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faArrowLeft, faArrowRight, faBars, faBell, faBolt, faChevronDown, faChevronUp, faCircleCheck, faCircleXmark, faFileImport, faFileLines, faFloppyDisk, faGear, faHouse, faMagnifyingGlass, faMinus, faPencil, faPlay, faPlus, faPuzzlePiece, faRightToBracket, faRotate, faStopwatch, faTableList, faToggleOff, faToggleOn, faTrash, faUsers, faWrench, faXmark } from '@fortawesome/free-solid-svg-icons';
import { CookieService } from 'ngx-cookie-service';
import { SharedModule } from './presentation/view/shared/shared.module';
import { faCalendar } from '@fortawesome/free-regular-svg-icons';

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
		SharedModule,
  		FontAwesomeModule
	],
	providers: [CookieService],
	bootstrap: [AppComponent]
})
export class AppModule {
	constructor(library: FaIconLibrary, faConfig: FaConfig) {
		faConfig.fixedWidth = true;
		library.addIcons(faRightToBracket, faCircleCheck, faCircleXmark, faWrench, faArrowLeft, faArrowRight, faBell, faMagnifyingGlass, faGear, faHouse, faPuzzlePiece, faBars, faXmark, faPlus, faFileImport, faUsers, faPencil, faTrash, faTableList, faFloppyDisk, faChevronUp, faChevronDown, faMinus, faPlay, faToggleOff, faToggleOn, faRotate, faFileLines, faStopwatch, faCalendar, faBolt);
	}
}
