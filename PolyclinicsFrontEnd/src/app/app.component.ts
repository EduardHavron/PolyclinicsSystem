import {AfterViewInit, Component, ElementRef, OnInit} from '@angular/core';
import {AuthorizationService} from "./shared/services/auth/authorization.service";
import {Login} from "./shared/models/authorize/login";
import {Observable} from "rxjs";
import {IsLoadingService} from "@service-work/is-loading";
import {NavigationCancel, NavigationEnd, NavigationError, NavigationStart, Router} from "@angular/router";
import {filter, map, shareReplay} from "rxjs/operators";
import {BreakpointObserver, Breakpoints} from "@angular/cdk/layout";
import {User} from "./shared/models/user/User";
import {Role} from "./shared/static/role";
import {MatSnackBar} from "@angular/material/snack-bar";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'PolyclinicsFrontEnd';
  public user: User | null
  public isLoading = new Observable<boolean>();
  constructor(public authService: AuthorizationService,
              private isLoadingService: IsLoadingService,
              private router: Router,
              private _snackBar: MatSnackBar) {
    this.user = null
    this.authService.currentUser.subscribe(user => this.user = user)
  }

  ngOnInit() {
    this.isLoading = this.isLoadingService.isLoading$();

    this.router.events
      .pipe(
        filter(
          event =>
            event instanceof NavigationStart ||
            event instanceof NavigationEnd ||
            event instanceof NavigationCancel ||
            event instanceof NavigationError,
        ),
      )
      .subscribe(event => {
        // If it's the start of navigation, `add()` a loading indicator
        if (event instanceof NavigationStart) {
          this.isLoadingService.add();
          return;
        }

        // Else navigation has ended, so `remove()` a loading indicator
        this.isLoadingService.remove();
      });
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
