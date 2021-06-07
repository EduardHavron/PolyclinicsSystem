import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule, Routes} from '@angular/router';
import {NotFoundComponent} from "../../not-found/not-found.component";
import {HomePageDoctorComponent} from "../home-page-doctor/home-page-doctor.component";
import {HomePagePatientComponent} from "../home-page-patient/home-page-patient.component";
import {HomePageAdminComponent} from "../home-page-admin/home-page-admin.component";

const routes: Routes = [
  {
    path: 'doctor',
    component: HomePageDoctorComponent
  },
  {
    path: 'patient',
    component: HomePagePatientComponent
  },
  {
    path: 'admin',
    component: HomePageAdminComponent
  },
  {
    path: '**',
    component: NotFoundComponent
  }
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
  ],
  exports: [
    RouterModule
  ]
})
export class HomeRouterModule {
}
