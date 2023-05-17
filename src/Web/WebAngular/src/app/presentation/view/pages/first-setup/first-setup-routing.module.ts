import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FirstSetupComponent } from './first-setup.component';

const routes: Routes = [
	{ path: "first-setup", component: FirstSetupComponent, data: { title: "First Setup", toolbar: false } }
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class FirstSetupRoutingModule { }
