import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ConnectorFunctionComponent } from './connector-function.component';
import { authGuard } from 'src/app/infra/auth/auth.guard';

const routes: Routes = [
	{ path: "connector-function", component: ConnectorFunctionComponent, canActivate: [authGuard], data: { title: "Connector function"} },
	{ path: "connector-function/:id", component: ConnectorFunctionComponent, canActivate: [authGuard], data: { title: "Connector function"} }
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class ConnectorFunctionRoutingModule { }
