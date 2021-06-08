import {Component, OnInit} from '@angular/core';
import {Doctor} from "../../../shared/models/doctor/doctor";
import {IsLoadingService} from "@service-work/is-loading";
import {AuthorizationService} from "../../../shared/services/auth/authorization.service";
import {MatSnackBar} from "@angular/material/snack-bar";

@Component({
  selector: 'app-home-page-admin',
  templateUrl: './home-page-admin.component.html',
  styleUrls: ['./home-page-admin.component.css']
})
export class HomePageAdminComponent implements OnInit {
  public dataSource: Array<Doctor> | null
  public displayedColumns: string[] = ['Name', 'Surname', 'Doctor Type', 'Email']
  constructor(private loadingService: IsLoadingService,
              private authService: AuthorizationService,
              private snackBar: MatSnackBar) {
    this.dataSource = null
    this.loadingService.add({key: 'homeAdmin'})
    this.authService.getDoctors().subscribe(data => {
      this.loadingService.remove({key: 'homeAdmin'})
      console.log(data)
      this.dataSource = data
    },
      error => {
        this.loadingService.remove({key: 'homeAdmin'})
        this.snackBar.open("An error appeared while fetching doctors", "Error", {
          duration: 3000
        })
      })
  }

  ngOnInit(): void {
  }

}
