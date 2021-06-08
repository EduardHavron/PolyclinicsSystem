import {Treatment} from "../treatment/treatment";

export class Diagnose {
  public diagnoseId: number = -1
  public diagnoseInfo: string = ''
  public diagnoseDate: string = ''
  public treatment: Treatment = new Treatment()
}
