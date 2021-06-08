import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {HomeRouterModule} from "../home-router/home-router.module";
import {MatTableModule} from "@angular/material/table";
import {MatCheckboxModule} from "@angular/material/checkbox";
import {HomePageDoctorComponent} from "../home-page-doctor/home-page-doctor.component";
import {HomePageAdminComponent} from "../home-page-admin/home-page-admin.component";
import {HomePagePatientComponent} from "../home-page-patient/home-page-patient.component";
import {IsLoadingModule} from "@service-work/is-loading";
import {MatIconModule} from "@angular/material/icon";
import {MatProgressBarModule} from "@angular/material/progress-bar";
import {MatButtonModule} from "@angular/material/button";


@NgModule({
  declarations: [
    HomePageDoctorComponent,
    HomePageAdminComponent,
    HomePagePatientComponent
  ],
  imports: [
    CommonModule,
    HomeRouterModule,
    MatTableModule,
    MatCheckboxModule,
    IsLoadingModule,
    MatIconModule,
    MatProgressBarModule,
    MatButtonModule
  ]
})
export class HomeModule { }
