import {Component, OnInit} from '@angular/core';
import {Appointment} from "../../../shared/models/appointment/appointment";
import {AppointmentsService} from "../../../shared/services/appointments/appointments.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Router} from "@angular/router";
import {AuthorizationService} from "../../../shared/services/auth/authorization.service";
import {User} from "../../../shared/models/user/User";
import {AppointmentStatus} from "../../../shared/static/appointment-status/appointment-status";
import {IsLoadingService} from "@service-work/is-loading";
import {AppointmentStatuses} from "../../../shared/enums/appointment-statuses";

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page-doctor.component.html',
  styleUrls: ['./home-page-doctor.component.css']
})
export class HomePageDoctorComponent implements OnInit {
  public appointments: Array<Appointment> | null
  public dataSource: Array<Appointment> | null
  private user: User | null
  public appointmentStatus = new AppointmentStatus()
  public displayedColumns: string[] = ['Status', 'Name', 'Surname', 'Diagnose settled up', 'View', 'Cancel', 'Time']

  constructor(private appointmentService: AppointmentsService,
              private authService: AuthorizationService,
              private snackBar: MatSnackBar,
              private router: Router,
              private isLoadingService: IsLoadingService) {
    this.dataSource = null
    this.appointments = null
    this.user = null
    this.isLoadingService.add({key: 'homeDoctor'})
    this.authService.currentUser.subscribe(user => {
      this.user = user
      if(this.user != null && this.user) {
        this.appointmentService.getAppointmentsForDoctor(this.user.id, true)
          .subscribe(appointments => {
              this.isLoadingService.remove({key: 'homeDoctor'})
              this.appointments = appointments
            this.dataSource = appointments.filter(x => {
              return (new Date(x.appointmentDate).getDate() === new Date().getDate() && x.appointmentStatus !== AppointmentStatuses.Finalized)
            })
          },
            error => {
              this.isLoadingService.remove({key: 'homeDoctor'})
              this.snackBar.open("Cannot load appointments", "Error", {
              duration: 5000
            })
            })
      }
    },
      error => {
        this.isLoadingService.remove({key: 'homeDoctor'})
        this.snackBar.open("Cannot load current user", "Error",
        {
          duration: 5000
        })
      })
  }

  public checkForCancelAbility(appointmentStatus: AppointmentStatuses) {
    return appointmentStatus === AppointmentStatuses.Planned
  }

  public cancelAppointment(appointmentId: number) {
    this.isLoadingService.add({key: 'homePatient'})
    this.appointmentService.cancelAppointment(appointmentId)
      .subscribe(() => {
          if (this.dataSource != null) {
            this.dataSource = this.dataSource.filter(x => {
              return x.appointmentId !== appointmentId
            })
          }
          this.snackBar.open("Appointment successfully canceled", "Information", {
              duration: 5000
            }
          )
          this.isLoadingService.remove({key: 'homePatient'})
        },
        () => {
          this.snackBar.open("An error appeared while cancelling appointment", "Error", {
              duration: 5000
            }
          )
        })
  }

  ngOnInit(): void {
  }

}
