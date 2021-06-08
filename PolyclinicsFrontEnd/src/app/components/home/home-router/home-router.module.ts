import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule, Routes} from '@angular/router';
import {NotFoundComponent} from "../../not-found/not-found.component";
import {HomePageDoctorComponent} from "../home-page-doctor/home-page-doctor.component";
import {HomePagePatientComponent} from "../home-page-patient/home-page-patient.component";
import {HomePageAdminComponent} from "../home-page-admin/home-page-admin.component";
import {DoctorGuard} from "../../../shared/guard/doctor/doctor.guard";
import {PatientGuard} from "../../../shared/guard/patient/patient.guard";
import {AdminGuard} from "../../../shared/guard/admin/admin.guard";
import {HomeRedirectGuard} from "../../../shared/guard/home-redirect/home-redirect.guard";

const routes: Routes = [
  {
    path: '',
    canActivate: [HomeRedirectGuard],
    canLoad: [HomeRedirectGuard]
  },
  {
    path: 'doctor',
    component: HomePageDoctorComponent,
    canActivate: [DoctorGuard],
    canLoad: [DoctorGuard]
  },
  {
    path: 'patient',
    component: HomePagePatientComponent,
    canActivate: [PatientGuard],
    canLoad: [PatientGuard]
  },
  {
    path: 'admin',
    component: HomePageAdminComponent,
    canActivate: [AdminGuard],
    canLoad: [AdminGuard]
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
