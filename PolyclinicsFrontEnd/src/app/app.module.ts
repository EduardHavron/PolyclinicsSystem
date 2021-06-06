import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {MatToolbarModule} from '@angular/material/toolbar'
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import {HttpClientModule} from "@angular/common/http";
import {RouterModule} from "@angular/router";
import { LayoutModule } from '@angular/cdk/layout';
import { MatIconModule } from '@angular/material/icon';
import { AppRoutingModule } from './app-routing.module';
import { MatCardModule } from '@angular/material/card';
import {IsLoadingModule} from "@service-work/is-loading";
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";


@NgModule({
  declarations: [
    AppComponent,
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
    MatProgressSpinnerModule,
    MatIconModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
