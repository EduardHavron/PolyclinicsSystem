import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {AuthorizationService} from "../../../shared/services/auth/authorization.service";
import {Router} from "@angular/router";
import {IsLoadingService} from "@service-work/is-loading";
import {MyErrorStateMatcher} from "../../../shared/validators/my-error-state-matcher";
import {MatSnackBar} from "@angular/material/snack-bar";
import {User} from "../../../shared/models/user/User";
import {Role} from "../../../shared/static/role/role";
import {Roles} from "../../../shared/enums/roles";


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})


export class LoginComponent implements OnInit {
  public loginForm: FormGroup;
  public isPasswordDisplayed = false
  public user: User | null
  public email = new FormControl('', [
    Validators.required,
    Validators.email,
  ]);
  public password = new FormControl('', [
    Validators.required
  ]);
  public matcher = new MyErrorStateMatcher()

  constructor(private formBuilder: FormBuilder,
              private authService: AuthorizationService,
              private router: Router,
              private isLoadingService: IsLoadingService,
              private _snackBar: MatSnackBar) {
    this.user = null
    this.authService.currentUser.subscribe(user => this.user = user)
    this.loginForm = this.formBuilder.group({
      email: this.email,
      password: this.password
    });
  }

  ngOnInit(): void {
  }

  public login() {
    const val = this.loginForm.value;
    if (val.email && val.password) {
      this.isLoadingService.add({key: 'login'})

      this.authService.signIn({email: val.email, password: val.password})
        .subscribe(
          () => {
            let route = ''
            if (this.user?.roles.includes(Role.getEnumString(Roles.Doctor)))
               route = 'doctor'
            else if (this.user?.roles.includes(Role.getEnumString(Roles.Patient)))
               route = 'patient'
            else  route = 'admin'

            this.router.navigate([`/home/${route}`])
              .then(() => {
                this.isLoadingService.remove({key: 'login'})
                this._snackBar.open(`Successfully authorized as ${val.email}`, 'Success',
                  {
                    duration: 5000
                  })
              });
          },
          error => {
            this.isLoadingService.remove({key: 'login'})
            this._snackBar.open("An error appeared while attempting to log in. Verify data and try again", 'Error',
              {
                duration: 5000
              })
          }
        );
    }
  }

  public changePasswordVisibility() {
    this.isPasswordDisplayed = !this.isPasswordDisplayed
  }
}
