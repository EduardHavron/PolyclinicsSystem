import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormControl, FormGroup, FormGroupDirective, NgForm, Validators} from "@angular/forms";
import {ErrorStateMatcher} from "@angular/material/core";
import {AuthorizationService} from "../../../shared/services/auth/authorization.service";
import {Router} from "@angular/router";
export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})


export class LoginComponent implements OnInit {
  public loginForm: FormGroup;
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
              private router: Router) {

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
      this.authService.signIn({email: val.email, password: val.password})
        .subscribe(
          () => {
            this.router.navigate(['/dashboard'])
              .then(() => {

              });
          }
        );
    }
  }
}
