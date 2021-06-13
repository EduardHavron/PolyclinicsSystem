import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {MatToolbarModule} from '@angular/material/toolbar'
import {AppComponent} from './app.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {RouterModule} from "@angular/router";
import {LayoutModule} from '@angular/cdk/layout';
import {MatIconModule} from '@angular/material/icon';
import {AppRoutingModule} from './app-routing.module';
import {IsLoadingModule} from "@service-work/is-loading";
import {MatProgressBarModule} from "@angular/material/progress-bar";
import {MatMenuModule} from "@angular/material/menu";
import {MatButtonModule} from "@angular/material/button";
import {MatTableModule} from "@angular/material/table";
import {JwtInterceptor} from "./shared/interceptors/jwt/jwt.interceptor";
import { ViewAppointmentComponent } from './components/appointments/view-appointment/view-appointment.component';
import { AllAppointmentsComponent } from './components/appointments/all-appointments/all-appointments.component';
import {MatGridListModule} from "@angular/material/grid-list";
import { ViewMedicalCardComponent } from './components/appointments/view-medical-card/view-medical-card.component';
import {MatCardModule} from "@angular/material/card";
import {ReactiveFormsModule} from "@angular/forms";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {MatExpansionModule} from "@angular/material/expansion";


@NgModule({
  declarations: [
    AppComponent,
    ViewAppointmentComponent,
    AllAppointmentsComponent,
    ViewMedicalCardComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MatSnackBarModule,
    HttpClientModule,
    RouterModule,
    LayoutModule,
    RouterModule,
    AppRoutingModule,
    IsLoadingModule,
    MatProgressBarModule,
    MatIconModule,
    MatMenuModule,
    MatButtonModule,
    MatTableModule,
    MatToolbarModule,
    MatGridListModule,
    MatCardModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatExpansionModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    ],
  bootstrap: [AppComponent]
})
export class AppModule { }
