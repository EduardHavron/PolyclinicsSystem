import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {MatToolbarModule} from '@angular/material/toolbar'
import {AppComponent} from './app.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import {HttpClientModule} from "@angular/common/http";
import {RouterModule} from "@angular/router";
import {LayoutModule} from '@angular/cdk/layout';
import {MatIconModule} from '@angular/material/icon';
import {AppRoutingModule} from './app-routing.module';
import {MatCardModule} from '@angular/material/card';
import {IsLoadingModule} from "@service-work/is-loading";
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";
import {HomePageDoctorComponent} from './components/home/home-page-doctor/home-page-doctor.component';
import {ReactiveFormsModule} from "@angular/forms";
import {MatProgressBarModule} from "@angular/material/progress-bar";
import {MatExpansionModule} from "@angular/material/expansion";
import {MatDividerModule} from "@angular/material/divider";
import {MatListModule} from "@angular/material/list";
import {MatMenuModule} from "@angular/material/menu";
import {MatButtonModule} from "@angular/material/button";
import { HomePageAdminComponent } from './components/home/home-page-admin/home-page-admin.component';
import { HomePagePatientComponent } from './components/home/home-page-patient/home-page-patient.component';


@NgModule({
  declarations: [
    AppComponent,
    HomePageDoctorComponent,
    HomePageAdminComponent,
    HomePagePatientComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MatSnackBarModule,
    HttpClientModule,
    MatToolbarModule,
    RouterModule,
    LayoutModule,
    RouterModule,
    AppRoutingModule,
    MatCardModule,
    IsLoadingModule,
    MatProgressBarModule,
    MatIconModule,
    ReactiveFormsModule,
    MatMenuModule,
    MatButtonModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
