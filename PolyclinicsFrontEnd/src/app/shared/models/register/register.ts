import {Roles} from "../../enums/roles";

export class Register {
  public email: string = ''
  public password: string = ''
  public firstName: string = ''
  public lastName: string = ''
  public role: Roles = Roles.Unauthorized
  public doctorType?: string
}
