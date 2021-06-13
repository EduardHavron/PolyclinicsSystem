import {Component, ElementRef, HostListener, OnInit, ViewChild} from '@angular/core';
import {AppointmentsService} from "../../../shared/services/appointments/appointments.service";
import {AuthorizationService} from "../../../shared/services/auth/authorization.service";
import {User} from "../../../shared/models/user/User";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Appointment} from "../../../shared/models/appointment/appointment";
import {Role} from "../../../shared/static/role/role";
import {Roles} from "../../../shared/enums/roles";
import {IsLoadingService} from "@service-work/is-loading";
import {AppointmentStatuses} from "../../../shared/enums/appointment-statuses";
import {AppointmentStatus} from "../../../shared/static/appointment-status/appointment-status";

@Component({
  selector: 'app-all-appointments',
  templateUrl: './all-appointments.component.html',
  styleUrls: ['./all-appointments.component.css']
})
export class AllAppointmentsComponent implements OnInit {

  public user = new User()
  private appointments = new Array<Appointment>()
  public dataSource = new Array<Appointment>()
  public displayedColumnsForDoctor: string[] = ['Status', 'Patient Name', 'Patient Surname', 'Diagnose settled up', 'View', 'Cancel', 'Time', 'Date']
  public displayedColumnsForPatient: string[] = ['Status', 'Doctor Name', 'Doctor Surname', 'Diagnose settled up', 'View', 'Reschedule', 'Cancel', 'Time', 'Date']
  public appointmentStatus = new AppointmentStatus()
  constructor(private appointmentService: AppointmentsService,
              private authService: AuthorizationService,
              private snackBar: MatSnackBar,
              private isLoading: IsLoadingService) {
    this.isLoading.add({key: 'allAppointments'})
  this.authService.currentUser.subscribe(data => {
    if (data != null) this.user = data
    if (this.user.roles.includes(Role.getEnumString(Roles.Doctor))) {
      this.appointmentService.getAppointmentsForDoctor(this.user.id, true)
        .subscribe(data => {
          this.appointments = data
          this.dataSource = this.appointments
          this.isLoading.remove({key: 'allAppointments'})
        }, () => {
          this.snackBar.open("Failed to retrieve appointments from the server", "Error", {
            duration: 5000
          })
        })
    } else {
      this.appointmentService.getAppointmentsForPatient(this.user.id, true)
        .subscribe(data => {
          this.appointments = data
          this.dataSource = this.appointments
          this.isLoading.remove({key: 'allAppointments'})
        }, () => {
          this.snackBar.open("Failed to retrieve appointments from the server", "Error", {
            duration: 5000
          })
        })
    }
  }, () => {
    snackBar.open("Failed to retrieve user data", "Error", {
      duration: 5000
    })
  })
  }

  ngOnInit(): void {
  }

  public checkIfPatient() {
    return this.user.roles.includes(Role.getEnumString(Roles.Patient))
  }

  public checkIfDoctor() {
    return this.user.roles.includes(Role.getEnumString(Roles.Doctor))
  }

  public checkForCancelAbility(appointmentStatus: AppointmentStatuses) {
    return appointmentStatus === AppointmentStatuses.Planned
  }

  public cancelAppointment(appointmentId: number) {
    this.isLoading.add({key: 'homePatient'})
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
          this.isLoading.remove({key: 'homePatient'})
        },
        () => {
          this.snackBar.open("An error appeared while cancelling appointment", "Error", {
              duration: 5000
            }
          )
        })
  }

}
