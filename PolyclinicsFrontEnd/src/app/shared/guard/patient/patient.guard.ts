import {Injectable} from '@angular/core';
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
import {Observable} from 'rxjs';
import {Role} from "../../static/role/role";
import {Roles} from "../../enums/roles";
import {AuthorizationService} from "../../services/auth/authorization.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {User} from "../../models/user/User";

@Injectable({
  providedIn: 'root'
})
export class PatientGuard implements CanActivate, CanLoad {
  private user: User | null
  constructor(private authService: AuthorizationService,
              private router: Router,
              private _snackBar: MatSnackBar) {
    this.user = null
    this.authService.currentUser.subscribe(user => this.user = user)

  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.verifyAccess();
  }
  canLoad(
    route: Route,
    segments: UrlSegment[]): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.verifyAccess();
  }

  private verifyAccess() {
    if (this.user != null && this.user.token.length > 0 && this.user.roles.includes(Role.getEnumString(Roles.Patient))) {
      return true
    }
    this.router.navigate(['/home'])
      .then(() => {
        this._snackBar.open("You should be patient to view this page", "Error", {
          duration: 5000
        })
      })
    return false
  }
}
