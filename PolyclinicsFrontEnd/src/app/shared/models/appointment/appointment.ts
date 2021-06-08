import {AppointmentStatuses} from "../../enums/appointment-statuses";
import {Doctor} from "../doctor/doctor";
import {Patient} from "../patient/patient";
import {Diagnose} from "../diagnose/diagnose";

export class Appointment {
  public appointmentId: number = -1
  public appointmentStatus: AppointmentStatuses = AppointmentStatuses.NotExist
  public doctor: Doctor = new Doctor()
  public patient: Patient = new Patient()
  public diagnose?: Diagnose = new Diagnose()
  public appointmentDate: string = ''
}
