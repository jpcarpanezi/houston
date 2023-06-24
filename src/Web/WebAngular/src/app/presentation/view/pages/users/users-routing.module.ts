import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UsersComponent } from './users.component';
import { authGuard } from 'src/app/infra/auth/auth.guard';

const routes: Routes = [
	{ path: "users", component: UsersComponent, canActivate: [authGuard], data: { title: "Users"} }
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class UsersRoutingModule { }
