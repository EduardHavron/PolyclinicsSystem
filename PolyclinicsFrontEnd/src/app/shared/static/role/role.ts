import {Roles} from "../../enums/roles";

export class Role {
  public static readonly unauthorized = "Unauthorized"
  public static readonly patient = "Patient";
  public static readonly doctor = "Doctor";
  public static readonly admin = "Admin";

  public static getEnumValue(role: string): Roles {
    switch (role) {
      case Role.patient:
        return Roles.Patient
      case Role.doctor:
        return Roles.Doctor
      case Role.admin:
        return Roles.Admin
      default:
        return Roles.Unauthorized
    }
  }

  public static getEnumString(role: Roles): string {
    switch (role) {
      case Roles.Admin:
        return Role.admin
      case Roles.Doctor:
        return Role.doctor
      case Roles.Patient:
        return Role.patient
      default:
        return Role.unauthorized
    }
  }
}
