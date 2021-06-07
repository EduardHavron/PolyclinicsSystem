import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {NotFoundComponent} from "./components/not-found/not-found.component";
import {UnauthGuard} from "./shared/guard/unauth/unauth.guard";
import {AuthGuard} from "./shared/guard/auth/auth.guard";
import {RegisterComponent} from "./components/authorize/register/register.component";

const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path: 'home',
    loadChildren: () => import('./components/home/home/home.module')
      .then(m => m.HomeModule),
    canLoad: [AuthGuard],
    canActivate:  [AuthGuard]
  },
  {
    path: 'admin/register',
    component: RegisterComponent
  },
  {
    path: 'authorize',
    loadChildren: () => import('./components/authorize/authorize/authorize.module')
      .then(m => m.AuthorizeModule),
    canLoad: [UnauthGuard],
    canActivate: [UnauthGuard]
  },
  {
    path: '**',
    component: NotFoundComponent
  }
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }
