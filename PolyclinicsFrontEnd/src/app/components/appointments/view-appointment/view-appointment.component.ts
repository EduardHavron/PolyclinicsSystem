import {Component, NgZone, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {AppointmentsService} from "../../../shared/services/appointments/appointments.service";
import {DiagnoseService} from "../../../shared/services/diagnose/diagnose.service";
import {TreatmentService} from "../../../shared/services/treatment/treatment.service";
import {MedicalCardService} from "../../../shared/services/medical-card/medical-card.service";
import {Appointment} from "../../../shared/models/appointment/appointment";
import {MedicalCard} from "../../../shared/models/medical-card/medical-card";
import {MatSnackBar} from "@angular/material/snack-bar";
import {IsLoadingService} from "@service-work/is-loading";
import {AppointmentStatus} from "../../../shared/static/appointment-status/appointment-status";
import {AppointmentStatuses} from "../../../shared/enums/appointment-statuses";
import {FormControl, Validators} from "@angular/forms";
import {CdkTextareaAutosize} from "@angular/cdk/text-field";
import {take} from "rxjs/operators";
import {AuthorizationService} from "../../../shared/services/auth/authorization.service";
import {User} from "../../../shared/models/user/User";
import {Role} from "../../../shared/static/role/role";
import {Roles} from "../../../shared/enums/roles";

@Component({
  selector: 'app-view-appointment',
  templateUrl: './view-appointment.component.html',
  styleUrls: ['./view-appointment.component.css']
})
export class ViewAppointmentComponent implements OnInit {
  public appointment: Appointment = new Appointment()
  public medicalCard: MedicalCard = new MedicalCard()
  public appointmentDateView: string
  public appointmentStatusView: string = AppointmentStatus.getEnumString(AppointmentStatuses.NotExist)
  public formGroupDiagnose: FormControl
  public formGroupTreatment: FormControl
  public currentUser: User = new User()
  @ViewChild('autosize') autosize: CdkTextareaAutosize | undefined;
  @ViewChild('diagnose') diagnose: any;
  @ViewChild('treatment') treatment: any;
  constructor(private router: Router,
              private snackBar: MatSnackBar,
              private isLoading: IsLoadingService,
              private activatedRoute: ActivatedRoute,
              private appointmentsService: AppointmentsService,
              private diagnoseService: DiagnoseService,
              private treatmentService: TreatmentService,
              private medicalCardService: MedicalCardService,
              private _ngZone: NgZone,
              private authService: AuthorizationService) {
    this.isLoading.add({key: 'viewAppointment'})
    this.formGroupDiagnose = new FormControl(
      '', Validators.required
    )
    this.formGroupTreatment = new FormControl('', Validators.required)
    this.appointmentDateView = ''

    const appointmentId = this.activatedRoute.snapshot.params['id']
    if (appointmentId != null) {
      this.authService.currentUser.subscribe(data => {
        if (data != null) {
          this.currentUser = data
          this.appointmentsService.getAppointment(appointmentId, true)
            .subscribe(data => {
                this.appointment = data
                this.appointmentDateView = new Date(data.appointmentDate).toLocaleString().substring(0, 17)
                this.appointmentStatusView = AppointmentStatus.getEnumString(data.appointmentStatus)
                if (data.diagnose !== null && data.diagnose !== undefined) {
                  if (data.diagnose.treatment != null) {
                    this.formGroupTreatment.setValue(
                      data.diagnose.treatment.treatmentInstructions)
                  }
                  this.formGroupDiagnose.setValue(
                    data.diagnose.diagnoseInfo
                  )
                }
                this.medicalCardService.getMedCard(data.patient.id, true)
                  .subscribe(data => {
                      this.medicalCard = data
                      this.isLoading.remove({key: 'viewAppointment'})
                    },
                    () => {
                      this.snackBar.open("Failed to load medical card for associated patient", 'Error', {
                        duration: 5000
                      })
                      this.isLoading.remove({key: 'viewAppointment'})
                    })
              },
              () => {
                this.snackBar.open("Failed to load appointment", 'Error', {
                  duration: 5000
                })
                this.isLoading.remove({key: 'viewAppointment'})
              })
        }
      }, () => {
        this.snackBar.open("Failed to load user", "Error", {
          duration: 5000
        })
        this.isLoading.remove({key: 'viewAppointment'})
      })
    }
  }

  ngOnInit(): void {
  }

  public checkIfDoctor() {
    return this.currentUser.roles.includes(Role.getEnumString(Roles.Doctor))
  }

  public checkIfPatient() {
    return this.currentUser.roles.includes(Role.getEnumString(Roles.Patient))
  }

  public checkIfStarted() {
    return this.appointment.appointmentStatus === AppointmentStatuses.Started
  }

  public checkIfDiagnoseSettled() {
    return this.appointment.diagnose !== null
  }

  public startAppointment() {
    if (this.appointment != null) {
      this.isLoading.add({key: 'viewAppointment'})
      this.appointmentsService.startAppointment(this.appointment.appointmentId)
        .subscribe(() => {
          this.snackBar.open("Appointment started, now you can set diagnose and treatment", "Information", {
            duration: 5000
          })
          if (this.appointment != null) {
            this.appointment.appointmentStatus = AppointmentStatuses.Started
          }
          this.appointmentStatusView = AppointmentStatus.getEnumString(AppointmentStatuses.Started)
          this.isLoading.remove({key: 'viewAppointment'})
        },
          () => {
            this.snackBar.open("Error appeared when starting appointment", "Error", {
              duration: 5000
            })
          })
    }
  }

  public finalizeAppointment() {
    if (this.appointment != null && this.appointment.diagnose != null && this.appointment.diagnose?.treatment != null)  {
      this.isLoading.add({key: 'viewAppointment'})
      this.appointmentsService.finalizeAppointment(this.appointment.appointmentId)
        .subscribe(() => {

            this.router.navigate(['/home'])
              .then(() => {
                this.snackBar.open("Appointment finalized", "Information", {
                  duration: 5000
                })
                this.isLoading.remove({key: 'viewAppointment'})
              })
          },
          () => {
            this.snackBar.open("Error appeared when finalizing appointment", "Error", {
              duration: 5000
            })
          })
    } else {
      this.snackBar.open("You can't finalize appointment without specifying diagnose and treatment", "Error", {
        duration: 5000
      })
    }
  }

  getErrorMessage(formGroup: FormControl): string {
    if (formGroup.hasError('required')) {
      return 'You must enter a value';
    }
    return ''
  }

  triggerResize() {
    // Wait for changes to be applied, then trigger textarea resize.

    this._ngZone.onStable.pipe(take(1))
      // @ts-ignore
      .subscribe(() => this.autosize.resizeToFitContent(true));
  }

  public saveDiagnose() {
    if (this.formGroupDiagnose.valid) {
      this.isLoading.add({key: 'viewAppointment'})
      if (this.appointment?.diagnose == null) {
        this.createDiagnose(<number>this.appointment?.appointmentId, <number>this.medicalCard?.medicalCardId)
      }
      else {
        this.updateDiagnose(this.appointment.diagnose.diagnoseId)
      }
    }
  }

  public saveTreatment() {
    if (this.formGroupTreatment.valid && this.formGroupDiagnose.valid) {
      this.isLoading.add({key: 'viewAppointment'})
      if (this.appointment?.diagnose != null && this.appointment.diagnose.treatment == null) {
        this.createTreatment(this.appointment.diagnose.diagnoseId)
      }
    else if (this.appointment?.diagnose != null && this.appointment.diagnose.treatment != null) {
      this.updateTreatment(this.appointment.diagnose.diagnoseId)
      }
    }
  }

  public createTreatment(diagnoseId: number) {
    this.treatmentService.createTreatment(diagnoseId, this.formGroupTreatment.value)
      .subscribe(() => {
        this.snackBar.open("Successfully created treatment, appointment can be finalized")
        setTimeout(() => {
          this.isLoading.remove({key: 'viewAppointment'})
          location.reload()
        }, 3000)
      })
  }

  public updateTreatment(diagnoseId: number) {
    this.treatmentService.createTreatment(diagnoseId, this.formGroupTreatment.value)
      .subscribe(() => {
        this.snackBar.open("Successfully updated treatment, appointment can be finalized")
        setTimeout(() => {
          this.isLoading.remove({key: 'viewAppointment'})
          location.reload()
        }, 3000)
      })
  }

  public createDiagnose(appointmentId: number, medicalCardId: number) {
    this.diagnoseService.addDiagnoseToCard(appointmentId, medicalCardId, this.formGroupDiagnose.value)
      .subscribe(() => {
        this.snackBar.open("Successfully created diagnose, please set treatment to get access to finalization")
        setTimeout(() => {
          this.isLoading.remove({key: 'viewAppointment'})
          location.reload()
        }, 3000)
      },
        () => {
          this.snackBar.open("An error appeared when creating diagnose", "Error", {
            duration: 5000
          })
          this.isLoading.remove({key: 'viewAppointment'})
        })
  }

  public updateDiagnose(diagnoseId: number) {
    this.diagnoseService.updateDiagnose(diagnoseId, this.formGroupDiagnose.value)
      .subscribe(() => {
        this.snackBar.open("Successfully updated diagnose")
        setTimeout(() => {
          this.isLoading.remove({key: 'viewAppointment'})
          location.reload()
        }, 3000)
      },
        () => {
          this.snackBar.open("An error appeared when upating diagnose", "Error", {
            duration: 5000
          })
          this.isLoading.remove({key: 'viewAppointment'})
        })
  }
}
