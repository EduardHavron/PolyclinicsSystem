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


@NgModule({
  declarations: [
    AppComponent,
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
    MatToolbarModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    ],
  bootstrap: [AppComponent]
})
export class AppModule { }
