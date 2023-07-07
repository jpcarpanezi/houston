import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ConnectorsComponent } from './connectors.component';
import { authGuard } from 'src/app/infra/auth/auth.guard';

const routes: Routes = [
	{ path: "connectors", component: ConnectorsComponent, canActivate: [authGuard], data: { title: "Connectors" } }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ConnectorsRoutingModule { }
