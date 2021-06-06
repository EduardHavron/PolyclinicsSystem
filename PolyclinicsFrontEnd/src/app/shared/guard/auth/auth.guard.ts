import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanLoad,
  Route,
  Router,
  RouterStateSnapshot,
  UrlSegment,
  UrlTree
} from '@angular/router';
import { Observable } from 'rxjs';
import {AuthorizationService} from "../../services/auth/authorization.service";
import {User} from "../../models/user/User";
import {MatSnackBar} from "@angular/material/snack-bar";

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate, CanLoad {
  private authUser: User | null
  constructor(private authService: AuthorizationService,
              private router: Router,
              private _snackBar: MatSnackBar) {
    this.authUser = null
    this.authService.currentUser.subscribe(user => this.authUser = user)
  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (this.authUser != null && this.authUser.token.length > 0) {
      return true
    }
    this.router.navigate(['/authorize'])
      .then(() => {
        this._snackBar.open("You should be authorized to view this page", "Error", {
          duration: 5000
        })
      })
    return false
  }
  canLoad(
    route: Route,
    segments: UrlSegment[]): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (this.authUser != null && this.authUser.token.length > 0) {
      return true
    }
    this.router.navigate(['/authorize'])
      .then(() => {
        this._snackBar.open("You should be authorized to view this page", "Error", {
          duration: 5000
        })
      })
    return false
  }
}
