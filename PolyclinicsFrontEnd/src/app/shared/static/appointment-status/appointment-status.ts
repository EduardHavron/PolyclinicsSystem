import {AppointmentStatuses} from "../../enums/appointment-statuses";

export class AppointmentStatus {
  public static readonly notExist: string = 'NotExist'
  public static readonly planned: string = 'Planned'
  public static readonly started: string = 'Started'
  public static readonly finalized: string = 'Finalized'
  public getEnumString(appointmentStatus: AppointmentStatuses): string {
    return AppointmentStatus.getEnumString(appointmentStatus)
  }
  public getEnumValue(appointmentStatus: string): AppointmentStatuses {
    return AppointmentStatus.getEnumValue(appointmentStatus)
  }

  public static getEnumValue(appointmentStatus: string): AppointmentStatuses {
    switch (appointmentStatus) {
      case this.planned:
        return AppointmentStatuses.Planned
      case this.started:
        return AppointmentStatuses.Started
      case this.finalized:
        return AppointmentStatuses.Finalized
      default:
        return AppointmentStatuses.NotExist
    }
  }

  public static getEnumString(appointmentStatus: AppointmentStatuses): string {
    switch (appointmentStatus) {
      case AppointmentStatuses.Planned:
        return this.planned
      case AppointmentStatuses.Started:
        return this.started
      case AppointmentStatuses.Finalized:
        return this.finalized
      default:
        return  this.notExist
    }
  }
}
