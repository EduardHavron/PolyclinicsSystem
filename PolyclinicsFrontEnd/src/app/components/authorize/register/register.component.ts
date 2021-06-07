import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {AuthorizationService} from "../../../shared/services/auth/authorization.service";
import {ActivatedRoute, Router} from "@angular/router";
import {Observable} from "rxjs";
import {IsLoadingService} from "@service-work/is-loading";
import {MyErrorStateMatcher} from "../../../shared/validators/my-error-state-matcher";
import {Register} from "../../../shared/models/register/register";
import {Roles} from "../../../shared/enums/roles";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Role} from "../../../shared/static/role";
import {User} from "../../../shared/models/user/User";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})


export class RegisterComponent implements OnInit {
  public registerForm: FormGroup;
  public isPasswordDisplayed = false
  public isLoading = new Observable<boolean>();
  public user: User | null
  public email = new FormControl('', [
    Validators.required,
    Validators.email,
  ]);
  public password = new FormControl('', [
    Validators.required
  ]);
  public firstName = new FormControl('', [
    Validators.required
  ])
  public lastName = new FormControl('', [
    Validators.required
  ])
  public doctorType = new FormControl('', [
    Validators.required
  ])
  public role = Roles.Patient
  public matcher = new MyErrorStateMatcher()

  constructor(private formBuilder: FormBuilder,
              private authService: AuthorizationService,
              private router: Router,
              private activatedRouteSnapshot: ActivatedRoute,
              private isLoadingService: IsLoadingService,
              private snackBar: MatSnackBar) {
    this.user = null
    this.authService.currentUser.subscribe(user => this.user = user)
    if (this.router.url.includes('admin')) {
      this.role = Roles.Doctor
    }
    this.registerForm = this.formBuilder.group({
      email: this.email,
      password: this.password,
      firstName: this.firstName,
      lastName: this.lastName,
    })

    if (this.role === Roles.Doctor) {
      this.registerForm.addControl('doctorType',this.doctorType)
    }

  }

  ngOnInit(): void {
  }

  public register() {
    const val = this.registerForm.value;
    if (this.registerForm.valid) {
      this.isLoadingService.add()
      let registerModel = new Register()
      registerModel.role = this.role
      registerModel.email = val.email
      registerModel.password = val.password
      registerModel.firstName = val.firstName
      registerModel.lastName = val.firstName
      if (this.role === Roles.Doctor)
        registerModel.doctorType = val.doctorType
        this.authService.signUp(registerModel, this.role === Roles.Patient)
          .subscribe(
            () => {
              this.isLoadingService.remove()
              let route = ''
              if (this.user?.roles.includes(Role.getEnumString(Roles.Doctor)))
                route = 'doctor'
              else if (this.user?.roles.includes(Role.getEnumString(Roles.Patient)))
                route = 'patient'
              else  route = 'admin'

              this.router.navigate([`/home/${route}`])
                .then(() => {
                  this.snackBar.open(`Successfully registered user ${val.email} ${this.role === Roles.Patient ? 'and authorized' : ''}`)
                });
            },
            error => {
              this.isLoadingService.remove()
              this.snackBar.open("An error appeared while attempting to register. Verify data and try again", 'Error',
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
