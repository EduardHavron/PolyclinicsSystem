import {Diagnose} from "../diagnose/diagnose";
import {Gender} from "../../enums/gender";

export class MedicalCard {
  public medicalCardId: number = -1
  public gender: Gender = Gender.Male
  public additionalInfo: string = ''
  public height: number = -1
  public weight: number = -1
  public age: number = -1
  public diagnoses: Array<Diagnose> = new Array<Diagnose>()
}
