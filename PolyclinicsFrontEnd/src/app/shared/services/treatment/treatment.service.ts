import {Injectable} from '@angular/core';
import {environment} from "../../../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {MedicalCard} from "../../models/medical-card/medical-card";

@Injectable({
  providedIn: 'root'
})
export class TreatmentService {
  private url = environment.apiUrl + 'treatment/'
  constructor(private http: HttpClient) { }

  public createTreatment(diagnoseId: number, treatment: string): Observable<MedicalCard> {
    return this.http.post<MedicalCard>(this.url + `create/${diagnoseId}`, {treatment})
  }

  public updateTreatment(treatmentId: number, treatment: string):Observable<MedicalCard> {
    return this.http.patch<MedicalCard>(this.url + `update/${treatmentId}`, {treatment})
  }

  public deleteTreatment(treatmentId: number): Observable<any> {
    return this.http.delete(this.url + `delete/${treatmentId}`)
  }
}
