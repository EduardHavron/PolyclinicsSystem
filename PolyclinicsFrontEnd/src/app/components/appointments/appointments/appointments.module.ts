import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {AppointmentsRouterModule} from "../appointments-router/appointments-router/appointments-router.module";
import {ScheduleComponent} from "../schedule/schedule.component";
import {MatGridListModule} from "@angular/material/grid-list";
import {MatStepperModule} from "@angular/material/stepper";
import {MatButtonModule} from "@angular/material/button";
import {MatCardModule} from "@angular/material/card";
import {MatInputModule} from "@angular/material/input";
import {ReactiveFormsModule} from "@angular/forms";
import {MatDatepickerModule} from "@angular/material/datepicker";
import {MatNativeDateModule} from "@angular/material/core";
import {FlexLayoutModule} from "@angular/flex-layout";
import {MatProgressBarModule} from "@angular/material/progress-bar";
import {IsLoadingPipeModule} from "@service-work/is-loading";
import {MatSelectModule} from "@angular/material/select";


@NgModule({
  declarations: [
    ScheduleComponent,
  ],
    imports: [
        CommonModule,
        AppointmentsRouterModule,
        MatGridListModule,
        MatStepperModule,
        MatButtonModule,
        MatCardModule,
        MatInputModule,
        ReactiveFormsModule,
        MatDatepickerModule,
        MatNativeDateModule,
        FlexLayoutModule,
        MatProgressBarModule,
        IsLoadingPipeModule,
        MatSelectModule
    ]
})
export class AppointmentsModule { }
