import {Component, OnInit, ViewChild} from '@angular/core';
import {Doctor} from "../../../shared/models/doctor/doctor";
import {AppointmentsService} from "../../../shared/services/appointments/appointments.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {AuthorizationService} from "../../../shared/services/auth/authorization.service";
import {Appointment} from "../../../shared/models/appointment/appointment";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {GroupedDateTime} from "../../../shared/models/date/grouped-date-time";
import {MatStepper} from "@angular/material/stepper";
import {MatCalendarCellClassFunction} from "@angular/material/datepicker";
import {IsLoadingService} from "@service-work/is-loading";
import {Router} from "@angular/router";
import {User} from "../../../shared/models/user/User";

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.css']
})
export class ScheduleComponent implements OnInit {

/*  public test: Array<Appointment> = [
    <Appointment>{appointmentDate: '2021-06-15T08:00:00+00:00'},
    <Appointment>{appointmentDate: '2021-06-16T08:00:00+00:00'},
    <Appointment>{appointmentDate: '2021-06-17T08:00:00+00:00'},
    <Appointment>{appointmentDate: '2021-06-18T08:00:00+00:00'},
    <Appointment>{appointmentDate: '2021-06-19T10:00:00+00:00'},

    <Appointment>{appointmentDate: '2021-06-15T08:30:00+00:00'},
    <Appointment>{appointmentDate: '2021-06-16T08:30:00+00:00'},
    <Appointment>{appointmentDate: '2021-06-17T08:30:00+00:00'},
    <Appointment>{appointmentDate: '2021-06-18T08:30:00+00:00'},
    <Appointment>{appointmentDate: '2021-06-19T08:30:00+00:00'},
    <Appointment>{appointmentDate: '2021-06-20T08:30:00+00:00'},

    <Appointment>{appointmentDate: '2021-06-15T10:30:00+00:00'},
    <Appointment>{appointmentDate: '2021-06-16T10:30:00+00:00'},
    <Appointment>{appointmentDate: '2021-06-17T10:30:00+00:00'},
    <Appointment>{appointmentDate: '2021-06-18T10:30:00+00:00'},
    <Appointment>{appointmentDate: '2021-06-19T10:30:00+00:00'},

    <Appointment>{appointmentDate: '2021-06-15T10:00:00+00:00'},
    <Appointment>{appointmentDate: '2021-06-16T10:00:00+00:00'},
    <Appointment>{appointmentDate: '2021-06-17T10:00:00+00:00'},
    <Appointment>{appointmentDate: '2021-06-18T10:00:00+00:00'},
    <Appointment>{appointmentDate: '2021-06-19T10:00:00+00:00'},
    <Appointment>{appointmentDate: '2021-06-20T10:00:00+00:00'},

  ]*/
  @ViewChild('stepper') stepper: MatStepper | undefined;
  public doctors: Array<Doctor> | null
  public chosenDoctor: Doctor | null
  public gridColumns = 4;
  public minDate = this.addDays(new Date(), 3)
  public maxDate = this.addDays(new Date, 30)
  public doctorFormGroup: FormGroup
  public dateFormGroup: FormGroup
  public timeFormGroup: FormGroup
  public occupiedRange: Array<GroupedDateTime>
  private patientId: User | null
  public validate = (date: Date | null) => {
    const occupiedRange = this.occupiedRange
    if (date == null) return false
    const occupiedDate = occupiedRange.find(x => {
      return new Date(x.date).toDateString() === new Date(date).toDateString()
    })
    if (occupiedDate == null) {
      return true
    }
    if (occupiedDate.times.length < occupiedDate.maxEntries) {
      return true
    } else  {
    }

    return false
  }

  public dateClass: MatCalendarCellClassFunction<Date> = (cellDate, view) => {
    const occupied = this.occupiedRange
    if (view === 'month') {
      const contains = occupied.find(x => {
        return new Date(x.date).toDateString() === new Date(cellDate).toDateString()
      })
      return (contains != null && contains.maxEntries <= contains.times.length) ? 'example-custom-date-class' : '';
    }

    return '';
  }
  public times: Array<{view: string, value: string }> = [
    {view: "08:00", value: "08:00:00"},
    {view: "08:30", value: "08:30:00"},
    {view: "09:00", value: "09:00:00"},
    {view: "09:30", value: "09:30:00"},
    {view: "10:00", value: "10:00:00"},
    {view: "10:30", value: "10:30:00"},
    {view: "11:00", value: "11:00:00"},
    {view: "11:30", value: "11:30:00"},
    {view: "12:00", value: "12:00:00"},
    {view: "12:30", value: "12:30:00"},
  ]
  public availableTimes = this.times
  constructor(private appointmentsService: AppointmentsService,
              private snackBar: MatSnackBar,
              private authService: AuthorizationService,
              private fb: FormBuilder,
              private isLoading: IsLoadingService,
              private router: Router) {
    this.isLoading.add({key: 'schedule'})
    this.patientId = null
    this.authService.currentUser.subscribe(user => this.patientId = user)
    this.occupiedRange = []
/*
    console.log(this.compositeData(this.test))
*/
    this.doctorFormGroup = this.fb.group( {
      doctor: new FormControl('', [
        Validators.required])
    })
    this.dateFormGroup = this.fb.group( {
      date: new FormControl('', [
        Validators.required
      ])
    })
    this.timeFormGroup = this.fb.group( {
      time: new FormControl('', [
        Validators.required
      ])
    })

    this.doctors = null
    this.chosenDoctor = null
    this.authService.getDoctors()
      .subscribe(data => {
        this.doctors = data
          this.isLoading.remove({key: 'schedule'})
        },
        () => {
        this.snackBar.open("Failed to retrieve doctors from server", 'Error', {
          duration: 5000
        })
          this.isLoading.remove({key: 'schedule'})
        })
  }

  ngOnInit(): void {
  }

  public chooseDoctor(doctorId: string) {
    this.isLoading.add({key: 'schedule'})
    if (this.doctors && this.doctors.length > 0) {
      const foundedDoctor =  this.doctors.find(x => x.id === doctorId)
      if (foundedDoctor) {
        this.chosenDoctor = foundedDoctor
        const control = this.doctorFormGroup.get('doctor')
        if (control) {
          this.doctorFormGroup.setValue({
            doctor: this.chosenDoctor.id
          })
          this.extractDoctorSchedule(this.chosenDoctor.id)
          this.isLoading.remove({key: 'schedule'})
          this.stepper?.next()
        }
      } else {
        this.snackBar.open("Failed to bind to chosen doctor", 'Error', {
          duration: 5000
        })
        this.isLoading.remove({key: 'schedule'})
      }
    }
  }

  public extractDoctorSchedule(doctorId: string) {
    let appointments = new Array<Appointment>()
    this.appointmentsService.getAppointmentsForDoctor(doctorId, true)
      .subscribe(data => {
        appointments = data
        this.occupiedRange = this.compositeData(appointments)
      })
  }

  public submitAppointment() {
    if (this.doctorFormGroup.valid && this.dateFormGroup.valid && this.timeFormGroup.valid) {
      this.isLoading.add({key: 'schedule'})
      //new Date(this.dateFormGroup.value.date).toLocaleString().split(',')[0]
      const appointmentDate = new Date(new Date(this.dateFormGroup.value.date).toDateString() + ' ' + this.timeFormGroup.value.time.split('+')[0])
      this.appointmentsService.createAppointment(this.doctorFormGroup.value.doctor,
        <string>this.patientId?.id, appointmentDate)
        .subscribe(() => {
          this.router.navigate(['/home'])
            .then(() => {
              this.snackBar.open("Successfully scheduled new appointment", "Information", {
                duration: 5000
              })
              this.isLoading.remove({key: 'schedule'})
            })
        },
          () => {
          this.snackBar.open("An error appeared when submitting appointment", 'Error', {
            duration: 5000
          })
            this.isLoading.remove({key: 'schedule'})
          })
    }
  }

  public addDays(date: Date, days: number) {
    const copy = new Date(Number(date))
    copy.setDate(date.getDate() + days)
    return copy
  }

  public compositeData(appointments: Array<Appointment>): Array<GroupedDateTime> {
    const dates = appointments.reduce<{[key: string]: Array<string>}>((groups, appointment) => {
      const date = appointment.appointmentDate.split('T')[0];
      if (groups[date] == null) {
        groups[date] = [];
      }
      groups[date].push(appointment.appointmentDate.split('T')[1]);
      return groups;
    }, {});

    return Object.keys(dates).map((date) => {
      return <GroupedDateTime>{
        date: date,
        maxEntries: 11,
        times: dates[date]
      };
    })
  }

  public filterTimes() {
    const date = this.occupiedRange.find( x => {
      return new Date(x.date).toDateString() === new Date(this.dateFormGroup.value.date).toDateString()
    })
    if (date == null)
      return
    this.availableTimes =  this.times.filter(x => {
      return !date.times.includes(x.value)
    })
  }
}
