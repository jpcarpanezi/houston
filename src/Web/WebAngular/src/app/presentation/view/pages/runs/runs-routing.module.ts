import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RunsComponent } from './runs.component';
import { authGuard } from 'src/app/infra/auth/auth.guard';

const routes: Routes = [
	{ path: "runs/:id", component: RunsComponent, canActivate: [authGuard], data: { title: "Runs"} }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RunsRoutingModule { }
