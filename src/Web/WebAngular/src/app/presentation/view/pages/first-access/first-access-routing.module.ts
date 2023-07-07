import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FirstAccessComponent } from './first-access.component';

const routes: Routes = [
	{ path: "first-access/:token", component: FirstAccessComponent, data: { title: "First Access", toolbar: false } }
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class FirstAccessRoutingModule { }
