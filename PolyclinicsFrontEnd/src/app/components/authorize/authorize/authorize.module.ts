import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {LoginComponent} from "../login/login.component";
import {RegisterComponent} from "../register/register.component";
import {AuthorizeRouterModule} from "../authorize-router/authorize-router.module";
import {MatCardModule} from "@angular/material/card";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {MatIconModule} from "@angular/material/icon";
import {MatButtonModule} from "@angular/material/button";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {IsLoadingModule} from "@service-work/is-loading";
import {MatProgressBarModule} from "@angular/material/progress-bar";
import {RouterModule} from "@angular/router";


@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent
  ],
  imports: [
    CommonModule,
    AuthorizeRouterModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    FormsModule,
    ReactiveFormsModule,
    MatProgressBarModule,
    IsLoadingModule,
    RouterModule
  ]
})
export class AuthorizeModule { }
