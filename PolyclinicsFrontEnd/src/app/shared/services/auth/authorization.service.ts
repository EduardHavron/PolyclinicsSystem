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
import {Doctor} from "../../models/doctor/doctor";

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {
  private currentUserSubject: BehaviorSubject<User | null>;
  public currentUser: Observable<User | null>
  private url = environment.apiUrl + 'account/';
  constructor(private http: HttpClient,
              private _snackBar: MatSnackBar) {
    this.currentUserSubject = new BehaviorSubject(JSON.parse(<string>localStorage.getItem('token')));
    this.currentUser = this.currentUserSubject.asObservable()
  }

  get isAuthorized(): boolean {
    const currentUser = this.currentUserSubject.getValue()

    return currentUser != null && currentUser.token.length > 0;
  }

  get isAdmin(): boolean {
    const currentUser = this.currentUserSubject.getValue();

    if (currentUser != null && currentUser.token.length > 0) {
      return currentUser.roles.includes(Role.admin);
    }

    return false;
  }

  get isPatient(): boolean {
    const currentUser = this.currentUserSubject.getValue();

    if (currentUser != null && currentUser.token.length > 0) {
      return currentUser.roles.includes(Role.patient);
    }

    return false;
  }

  get isDoctor(): boolean {
    const currentUser = this.currentUserSubject.getValue();
    if (currentUser != null && currentUser.token.length > 0) {
      return currentUser.roles.includes(Role.doctor);
    }
    return false;
  }

  signUp(user: Register, assignToUser =  false) : Observable<User> {
    return this.http.post<User>(this.url + 'register', user,
      {reportProgress: true})
      .pipe(
        tap(res => {
            if (res.token.length > 0) {
              if(assignToUser) {
                localStorage.setItem('token', JSON.stringify(res));
                this.currentUserSubject.next(res);
              }
            }
          },
          error => {
          })
      );
  }

  signIn(user: Login): Observable<User> {
    return this.http.post<User>(this.url + 'authorize', user, {
      reportProgress: true})
      .pipe(
        tap(res => {
          if (res.token.length > 0) {
            localStorage.setItem('token', JSON.stringify(res));
            this.currentUserSubject.next(res);
          }
        },
          error => {
          })
      );
  }

  getDoctors(): Observable<Array<Doctor>> {
    return this.http.get<Array<Doctor>>(this.url + 'getDoctors')
  }


  logout() {
    localStorage.removeItem('token');
    this.currentUserSubject.next(null);
  }

}
