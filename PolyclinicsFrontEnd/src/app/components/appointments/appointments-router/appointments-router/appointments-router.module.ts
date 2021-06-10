import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule, Routes} from '@angular/router';
import {NotFoundComponent} from "../../../not-found/not-found.component";
import {ScheduleComponent} from "../../schedule/schedule.component";
import {PatientGuard} from "../../../../shared/guard/patient/patient.guard";
import {ViewAppointmentComponent} from "../../view-appointment/view-appointment.component";
import {DoctorGuard} from "../../../../shared/guard/doctor/doctor.guard";
import {AllAppointmentsComponent} from "../../all-appointments/all-appointments.component";
import {Doctor} from "../../../../shared/models/doctor/doctor";


const routes: Routes = [
  {
    path: 'schedule',
    component: ScheduleComponent,
    canLoad: [PatientGuard],
    canActivate: [PatientGuard]
  },
  {
    path: 'reschedule/:id',
    component: ScheduleComponent,
    canLoad: [PatientGuard],
    canActivate: [PatientGuard]
  },
  {
    path: 'view-doctor/:id',
    component: ViewAppointmentComponent,
    canActivate: [DoctorGuard],
    canLoad: [DoctorGuard]
  },
  {
    path: 'view-patient/:id',
    component: ViewAppointmentComponent,
    canActivate: [PatientGuard],
    canLoad: [PatientGuard]
  },
  {
    path: 'all-doctor',
    component: AllAppointmentsComponent,
    canActivate: [DoctorGuard],
    canLoad: [DoctorGuard]
  },
  {
    path: 'all-patient',
    component: AllAppointmentsComponent,
    canActivate: [PatientGuard],
    canLoad: [PatientGuard]
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
export class AppointmentsRouterModule {
}
