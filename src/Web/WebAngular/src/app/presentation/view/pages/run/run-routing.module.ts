import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RunComponent } from './run.component';
import { authGuard } from 'src/app/infra/auth/auth.guard';

const routes: Routes = [
	{ path: "run/:id", component: RunComponent, canActivate: [authGuard], data: { title: "Run" } }
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class RunRoutingModule { }
