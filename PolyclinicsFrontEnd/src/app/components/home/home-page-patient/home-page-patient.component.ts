import {Component, OnInit} from '@angular/core';
import {Appointment} from "../../../shared/models/appointment/appointment";
import {User} from "../../../shared/models/user/User";
import {AppointmentStatus} from "../../../shared/static/appointment-status/appointment-status";
import {AppointmentsService} from "../../../shared/services/appointments/appointments.service";
import {AuthorizationService} from "../../../shared/services/auth/authorization.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Router} from "@angular/router";
import {IsLoadingService} from "@service-work/is-loading";
import {AppointmentStatuses} from "../../../shared/enums/appointment-statuses";

@Component({
  selector: 'app-home-page-patient',
  templateUrl: './home-page-patient.component.html',
  styleUrls: ['./home-page-patient.component.css']
})
export class HomePagePatientComponent implements OnInit {
  public appointments: Array<Appointment> | null
  public dataSource: Array<Appointment> | null
  private user: User | null
  public appointmentStatus = new AppointmentStatus()
  public displayedColumns: string[] = ['Status', 'Name', 'Surname', 'Diagnose settled up', 'Reschedule', 'Cancel', 'View', 'Time']

  constructor(private appointmentService: AppointmentsService,
              private authService: AuthorizationService,
              private snackBar: MatSnackBar,
              private router: Router,
              private isLoadingService: IsLoadingService) {
    this.dataSource = null
    this.appointments = null
    this.user = null
    this.isLoadingService.add({key: 'homePatient'})
    this.authService.currentUser.subscribe(user => {
        this.user = user
        if (this.user != null && this.user) {
          this.appointmentService.getAppointmentsForPatient(this.user.id, true)
            .subscribe(appointments => {
                this.appointments = appointments
                this.dataSource = appointments.filter(x => {
                  return (new Date(x.appointmentDate).getDate() === new Date().getDate() && x.appointmentStatus !== AppointmentStatuses.Finalized)
                })
                this.isLoadingService.remove({key: 'homePatient'})
              },
              error => {
                this.isLoadingService.remove({key: 'homePatient'})
                this.snackBar.open("Cannot load appointments", "Error", {
                  duration: 5000
                })
              })
        }
      },
      error => {
        this.isLoadingService.remove({key: 'homePatient'})
        this.snackBar.open("Cannot load current user", "Error",
          {
            duration: 5000
          })
      })
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

  public checkForCancelAbility(appointmentStatus: AppointmentStatuses) {
    return appointmentStatus === AppointmentStatuses.Planned
  }

  ngOnInit(): void {
  }

}
