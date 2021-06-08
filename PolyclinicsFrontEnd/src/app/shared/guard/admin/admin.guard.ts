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
import {AuthorizationService} from "../../services/auth/authorization.service";
import {User} from "../../models/user/User";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Role} from "../../static/role/role";
import {Roles} from "../../enums/roles";

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate, CanLoad {
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
    return this.verifyAccess()
  }
  canLoad(
    route: Route,
    segments: UrlSegment[]): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.verifyAccess();
  }

  private verifyAccess() {
    if (this.user != null && this.user.token.length > 0 && this.user.roles.includes(Role.getEnumString(Roles.Admin))) {
      return true
    }
    this.router.navigate(['/home'])
      .then(() => {
        this._snackBar.open("You should be administrator to view this page", "Error", {
          duration: 5000
        })
      })
    return false
  }
}
