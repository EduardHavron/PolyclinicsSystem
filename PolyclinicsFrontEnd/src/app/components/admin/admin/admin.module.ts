import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {AdminRouterModule} from "../admin-router/admin-router/admin-router.module";


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    AdminRouterModule
  ]
})
export class AdminModule { }
