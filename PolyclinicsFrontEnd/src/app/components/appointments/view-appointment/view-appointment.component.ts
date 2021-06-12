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
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {CdkTextareaAutosize} from "@angular/cdk/text-field";
import {take} from "rxjs/operators";

@Component({
  selector: 'app-view-appointment',
  templateUrl: './view-appointment.component.html',
  styleUrls: ['./view-appointment.component.css']
})
export class ViewAppointmentComponent implements OnInit {
  public appointment: Appointment | null
  public medicalCard: MedicalCard | null
  public appointmentDateView: string
  public appointmentStatusView: string
  public doctorNameView: string = ''
  public doctorTypeView: string = ''
  public patientNameView: string = ''
  public accessToActions: boolean = false
  public appointmentStatus = AppointmentStatuses.NotExist
  public formGroupDiagnose: FormGroup
  public formGroupTreatment: FormGroup
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
              private fb: FormBuilder) {
    this.isLoading.add({key: 'viewAppointment'})
    this.formGroupDiagnose = fb.group({
      diagnose: ['', Validators.required]
    })
    this.formGroupTreatment = fb.group({
      treatment: ['', Validators.required]
    })
    this.appointmentDateView = ''
    this.appointmentStatusView = ''
    this.appointment = null
    this.medicalCard = null
    const isDoctor = this.router.url.includes('view-doctor')

    const appointmentId = this.activatedRoute.snapshot.params['id']
    if (appointmentId != null) {
      this.appointmentsService.getAppointment(appointmentId, true)
        .subscribe(data => {
          this.appointment = data
          this.appointmentDateView = new Date(data.appointmentDate).toLocaleString().substring(0, 17)
          this.appointmentStatusView = AppointmentStatus.getEnumString(data.appointmentStatus)
          this.appointmentStatus = data.appointmentStatus
          this.doctorNameView = data.doctor.name + ' ' + data.doctor.surname
          this.doctorTypeView = data.doctor.doctorType
          this.patientNameView = data.patient.name + ' ' + data.patient.surname
          this.accessToActions = isDoctor && data.appointmentStatus === AppointmentStatuses.Started
          if (data.diagnose !== null && data.diagnose !== undefined) {
            if (data.diagnose.treatment != null) {
              this.formGroupTreatment.setValue({
                treatment: data.diagnose.treatment.treatmentInstructions
              })
            }
            this.formGroupDiagnose.setValue({
              diagnose: data.diagnose.diagnoseInfo
            })
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
  }

  ngOnInit(): void {
  }

  public startAppointment() {
    if (this.appointment != null) {
      this.isLoading.add({key: 'viewAppointment'})
      this.appointmentsService.startAppointment(this.appointment.appointmentId)
        .subscribe(() => {
          this.accessToActions = true
          this.snackBar.open("Appointment started, now you can set diagnose and treatment", "Information", {
            duration: 5000
          })
          this.appointmentStatus = AppointmentStatuses.Started
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
    if (this.appointment != null) {
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
    }
  }

  public checkForStartAbility() {
    return this.appointmentStatus === AppointmentStatuses.Planned
  }

  public checkForFinalizeAbility() {
    return (this.appointmentStatus === AppointmentStatuses.Started &&
      this.appointment?.diagnose != null &&
      this.appointment.diagnose.treatment != null)
  }


  getErrorMessage(formGroup: FormGroup): string {
    if (formGroup.hasError('required')) {
      return 'You must enter a value';
    }
    return ''
  }

  public checkTreatmentAbility() {
    return this.appointment != null &&
      this.appointment.appointmentStatus == AppointmentStatuses.Started &&
      this.appointment.diagnose != null
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
    this.treatmentService.createTreatment(diagnoseId, this.formGroupTreatment.value.treatment)
      .subscribe(() => {
        this.isLoading.remove({key: 'viewAppointment'})
        location.reload()
      })
  }

  public updateTreatment(diagnoseId: number) {
    this.treatmentService.createTreatment(diagnoseId, this.formGroupTreatment.value.treatment)
      .subscribe(() => {
        this.isLoading.remove({key: 'viewAppointment'})
        location.reload()
      })
  }

  public createDiagnose(appointmentId: number, medicalCardId: number) {
    this.diagnoseService.addDiagnoseToCard(appointmentId, medicalCardId, this.formGroupDiagnose.value.diagnose)
      .subscribe(() => {
        this.isLoading.remove({key: 'viewAppointment'})
        location.reload()
      })
  }

  public updateDiagnose(diagnoseId: number) {
    this.diagnoseService.updateDiagnose(diagnoseId, this.formGroupDiagnose.value.diagnose)
      .subscribe(() => {
        this.isLoading.remove({key: 'viewAppointment'})
        location.reload()
      })
  }
}
