import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { FormErrorsComponent } from './form-errors/form-errors.component';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { NgxLoaderIndicatorDirective, provideNgxLoaderIndicator } from 'ngx-loader-indicator';
import { provideAnimations } from '@angular/platform-browser/animations';
import { MiniMenuComponent } from './mini-menu/mini-menu.component';
import { MenuComponent } from './menu/menu.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { SettingsComponent } from './settings/settings.component';
import { SearchBarComponent } from './search-bar/search-bar.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { RouterModule } from '@angular/router';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { ModalComponent } from './modal/modal.component';



@NgModule({
	declarations: [
		FormErrorsComponent,
		MiniMenuComponent,
		MenuComponent,
		NotificationsComponent,
		SettingsComponent,
		SearchBarComponent,
  		SidebarComponent,
    ModalComponent
	],
	imports: [
		CommonModule,
		FontAwesomeModule,
		NgxLoaderIndicatorDirective,
		RouterModule
	],
	exports: [
		ReactiveFormsModule,
		FontAwesomeModule,
		SweetAlert2Module,
		FormErrorsComponent,
		NgxDatatableModule,
		NgxLoaderIndicatorDirective,
		SidebarComponent,
		ModalComponent
	],
	providers: [
		provideAnimations(),
		provideNgxLoaderIndicator()
	]
})
export class SharedModule { }
