import {Injectable} from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from '../../../../environments/environment';
import {AuthorizationService} from "../../services/auth/authorization.service";
import {User} from "../../models/user/User";

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  private currentUser: User | null
  constructor(private authenticationService: AuthorizationService) {
    this.currentUser = null
    this.authenticationService.currentUser.subscribe(user => {
      this.currentUser = user
    })
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const isLoggedIn = this.authenticationService.isAuthorized
    const isApiUrl = request.url.startsWith(environment.apiUrl);
    if (isLoggedIn && isApiUrl) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${this.currentUser?.token}`
        }
      });
    }

    return next.handle(request);
  }
}
