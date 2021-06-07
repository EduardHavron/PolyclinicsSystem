import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {HomeRouterModule} from "../home-router/home-router.module";
import {MatTableModule} from "@angular/material/table";
import {FormBuilder, FormsModule, ReactiveFormsModule} from "@angular/forms";


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    HomeRouterModule,
    MatTableModule,
  ]
})
export class HomeModule { }
