import {Component, OnInit} from '@angular/core';
import {AuthorizationService} from "./shared/services/auth/authorization.service";
import {NavigationCancel, NavigationEnd, NavigationError, NavigationStart, Router} from "@angular/router";
import {User} from "./shared/models/user/User";
import {MatSnackBar} from "@angular/material/snack-bar";
import {IsLoadingService} from "@service-work/is-loading";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit{
  title = 'PolyclinicsFrontEnd';
  public user: User | null
  constructor(public authService: AuthorizationService,
              private router: Router,
              private _snackBar: MatSnackBar,
              private isLoadingService: IsLoadingService) {
    this.user = null
    this.authService.currentUser.subscribe(user => this.user = user)
    this.router.events.subscribe((routerEvent => {
      if (routerEvent instanceof NavigationStart) {
       this.isLoadingService.add({key: 'app'})
      }

      if (routerEvent instanceof NavigationEnd ||
        routerEvent instanceof NavigationCancel ||
        routerEvent instanceof NavigationError) {
        setTimeout(() => {
          this.isLoadingService.remove({key: 'app'})
        }, 1500)
      }
    }));
  }

  ngOnInit() {
  }

  get isAuthorized(): boolean {
    return this.user != null && this.user.token.length > 0
  }

  get isAdmin(): boolean {
    return this.user != null && this.authService.isAdmin
  }

  get isPatient(): boolean {
    return this.user != null && this.authService.isPatient
  }

  get isDoctor(): boolean {
    return this.user != null && this.authService.isDoctor
  }

  public logout() {
    this.authService.logout()
    this.router.navigate(['/authorize/login'])
      .then(() => {
        this._snackBar.open("Successfully logged out", "Information", {
          duration: 5000
        })
      })
  }
}
