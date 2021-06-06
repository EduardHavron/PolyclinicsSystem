import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {BehaviorSubject, Observable} from 'rxjs';
import {tap} from 'rxjs/operators';
import {environment} from '../../../../environments/environment';
import {User} from "../../models/user/User";
import {Role} from "../../static/role";
import {Login} from "../../models/authorize/login";
import {Register} from "../../models/register/register";
import {MatSnackBar} from "@angular/material/snack-bar";

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>
  private url = environment.apiUrl + 'account/';
  constructor(private http: HttpClient,
              private _snackBar: MatSnackBar) {
    this.currentUserSubject = new BehaviorSubject
      <User> (new User);
    this.currentUser = this.currentUserSubject.asObservable()
  }

  get isAuthorized(): boolean {
    const currentUser = this.currentUserSubject.getValue()
    console.log("getted user value")
    return currentUser.token.length > 0;
  }

  get isAdmin(): boolean {
    const currentUser = this.currentUserSubject.getValue();

    if (currentUser.token.length > 0) {
      return currentUser.roles.includes(Role.admin);
    }

    return false;
  }

  get isPatient(): boolean {
    const currentUser = this.currentUserSubject.getValue();

    if (currentUser.token.length > 0) {
      return currentUser.roles.includes(Role.patient);
    }

    return false;
  }

  get isDoctor(): boolean {
    const currentUser = this.currentUserSubject.getValue();
    if (currentUser.token.length > 0) {
      return currentUser.roles.includes(Role.doctor);
    }
    return false;
  }

  signUp(user: Register) : Observable<User> {
    return this.http.post<User>(this.url + 'register', user,
      {reportProgress: true})
      .pipe(
        tap(res => {
            if (res.token.length > 0) {
              localStorage.setItem('token', JSON.stringify(res));
              this.currentUserSubject.next(res);
            }
          },
          error => {
            this._snackBar.open("An error appeared while attempting to register. Verify data and try again", 'Error',
              {
                duration: 5000
              })
          })
      );
  }

  signIn(user: Login): Observable<User> {
    return this.http.post<User>(this.url + 'authorize', user, {
      reportProgress: true})
      .pipe(
        tap(res => {
          if (res.token.length > 0) {
            localStorage.setItem('token', JSON.stringify(res.token));
            this.currentUserSubject.next(res);
            this._snackBar.open(`Successfully authorized as ${user.email}`, 'Success',
              {
                duration: 5000
              })
          }
        },
          error => {
          this._snackBar.open("An error appeared while attempting to log in. Verify data and try again", 'Error',
            {
              duration: 5000
            })
          })
      );
  }


  logout() {
    localStorage.removeItem('token');
    this.currentUserSubject.next(new User());
  }

}
