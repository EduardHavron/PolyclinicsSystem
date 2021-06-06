import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Route, Router, RouterStateSnapshot} from '@angular/router';
import {Observable} from 'rxjs';
import {AuthorizationService} from "../../services/auth/authorization.service";
import {User} from "../../models/user/User";
import {MatSnackBar} from "@angular/material/snack-bar";


@Injectable({providedIn: 'root'})
export class UnauthGuard implements CanActivate {
  private currentUser: User | null
  constructor(
    private router: Router,
    private authenticationService: AuthorizationService,
    private _snackBar: MatSnackBar
  ) {
    this.currentUser = null
    this.authenticationService.currentUser.subscribe(user => {
      this.currentUser = user
    })
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    if (this.currentUser == null || this.currentUser.token.length === 0) {
      return true;
    }
    else {

    }
    this.router.navigate(['/home'])
      .then(() => {
        this._snackBar.open(`You already authorized`,
          "Information", {
          duration: 5000
          })
      });
    return false;
  }

  canLoad(route: Route): Observable<boolean> | Promise<boolean> | boolean {

    if (this.currentUser == null || this.currentUser.token.length === 0) {
      return true;
    }

    this.router.navigate(['/dashboard'])
      .then(() => {
        this._snackBar.open(`You already authorized`,
          "Information", {
            duration: 5000
          })
      });
    return false;
  }
}
