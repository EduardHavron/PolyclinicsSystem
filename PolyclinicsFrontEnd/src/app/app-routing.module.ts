import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {NotFoundComponent} from "./components/not-found/not-found.component";
import {UnauthGuard} from "./shared/guard/unauth/unauth.guard";
import {AuthGuard} from "./shared/guard/auth/auth.guard";
import {AdminGuard} from "./shared/guard/admin/admin.guard";

const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path: 'appointments',
    loadChildren: () => import('./components/appointments/appointments/appointments.module')
      .then(m => m.AppointmentsModule),
    canLoad: [AuthGuard],
    canActivate: [AuthGuard]
  },
  {
    path: 'home',
    loadChildren: () => import('./components/home/home/home.module')
      .then(m => m.HomeModule),
    canLoad: [AuthGuard],
    canActivate:  [AuthGuard]
  },
  {
    path: 'admin',
    loadChildren: () => import('./components/admin/admin/admin.module')
      .then(m => m.AdminModule),
    canLoad: [AdminGuard],
    canActivate: [AdminGuard]
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
