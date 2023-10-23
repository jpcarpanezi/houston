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
import { MAT_TOOLTIP_DEFAULT_OPTIONS, MatTooltipModule } from '@angular/material/tooltip';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { NgxJdenticonModule } from 'ngx-jdenticon';
import { InfraModule } from 'src/app/infra/infra.module';
import { PaginatorComponent } from './paginator/paginator.component';
import { ConnectorsCardComponent } from './connectors-card/connectors-card.component';
import { ConnectorsModule } from '../pages/connectors/connectors.module';
import { NewConnectorComponent } from './connectors-card/new-connector/new-connector.component';
import { NewConnectorFunctionComponent } from './connectors-card/new-connector-function/new-connector-function.component';
import { UpdateConnectorComponent } from './connectors-card/update-connector/update-connector.component';
import { UpdateConnectorFunctionComponent } from './connectors-card/update-connector-function/update-connector-function.component';



@NgModule({
	declarations: [
		FormErrorsComponent,
		MiniMenuComponent,
		MenuComponent,
		NotificationsComponent,
		SettingsComponent,
		SearchBarComponent,
		SidebarComponent,
		ModalComponent,
		PaginatorComponent,
		ConnectorsCardComponent,
		NewConnectorComponent,
		NewConnectorFunctionComponent,
		UpdateConnectorComponent,
		UpdateConnectorFunctionComponent
	],
	imports: [
		CommonModule,
		FontAwesomeModule,
		NgxLoaderIndicatorDirective,
		RouterModule,
		NgxJdenticonModule,
		ReactiveFormsModule
	],
	exports: [
		ReactiveFormsModule,
		FontAwesomeModule,
		SweetAlert2Module,
		FormErrorsComponent,
		NgxDatatableModule,
		NgxLoaderIndicatorDirective,
		SidebarComponent,
		ModalComponent,
		ConnectorsCardComponent,
		PaginatorComponent,
		MatTooltipModule,
		DragDropModule,
		InfraModule
	],
	providers: [
		provideAnimations(),
		provideNgxLoaderIndicator({
			loaderStyles: {
				background: 'rgb(0, 0, 0, 0.5)'
			}
		}),
		{ provide: MAT_TOOLTIP_DEFAULT_OPTIONS, useValue: { position: "above" } }
	]
})
export class SharedModule { }
