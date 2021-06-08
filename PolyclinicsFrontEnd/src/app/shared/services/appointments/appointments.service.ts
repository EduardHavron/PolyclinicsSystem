import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../../environments/environment";
import {Observable} from "rxjs";
import {Appointment} from "../../models/appointment/appointment";

@Injectable({
  providedIn: 'root'
})
export class AppointmentsService {
  private url = environment.apiUrl + 'appointment/';
  constructor(private http: HttpClient) { }


  public getAppointment(appointmentId: number, includeDiagnose: boolean): Observable<Appointment> {
    return this.http.get<Appointment>(this.url + `getAppointment/${appointmentId}?includeDiagnose=${includeDiagnose}`)
  }

  public getAppointmentsForDoctor(doctorId: string, includeDiagnose: boolean): Observable<Array<Appointment>> {
    return this.http.get<Array<Appointment>>(this.url + `getAppointmentsDoctor/${doctorId}?includeDiagnose=${includeDiagnose}`)
  }

  public getAppointmentsForPatient(patientId: string, includeDiagnose: boolean): Observable<Array<Appointment>> {
    return this.http.get<Array<Appointment>>(this.url + `getAppointmentsPatient/${patientId}?includeDiagnose=${includeDiagnose}`)
  }

  public createAppointment(doctorId: string, patientId: string, appointmentDate: string): Observable<Appointment> {
    return this.http.post<Appointment>(this.url + `create`, {doctorId, patientId, appointmentDate})
  }

  public rescheduleAppointment(appointmentId: number, newDate: string): Observable<Appointment> {
    return this.http.patch<Appointment>(this.url + `reschedule/${appointmentId}`, {appointmentId, newDate})
  }

  public startAppointment(appointmentId: number): Observable<Appointment> {
    return this.http.patch<Appointment>(this.url + `start/${appointmentId}`, {})
  }

  public finalizeAppointment(appointmentId: number): Observable<Appointment> {
    return this.http.patch<Appointment>(this.url + `finalize/${appointmentId}`, {})
  }

  public cancelAppointment(appointmentId: number):Observable<any> {
    return this.http.delete(this.url + `cancel/${appointmentId}`)
  }
}
