import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule, Routes} from '@angular/router';
import {NotFoundComponent} from "../../../not-found/not-found.component";
import {ScheduleComponent} from "../../schedule/schedule.component";
import {PatientGuard} from "../../../../shared/guard/patient/patient.guard";


const routes: Routes = [
  {
    path: 'schedule',
    component: ScheduleComponent,
    canLoad: [PatientGuard],
    canActivate: [PatientGuard]
  },
  {
    path: 'view-doctor'
  },
  {
    path: 'view-patient'
  },
  {
    path: 'all-doctor'
  },
  {
    path: 'all-patient'
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
