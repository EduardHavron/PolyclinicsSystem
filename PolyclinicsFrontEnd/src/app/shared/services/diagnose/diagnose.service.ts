import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {environment} from "../../../../environments/environment";
import {MedicalCard} from "../../models/medical-card/medical-card";

@Injectable({
  providedIn: 'root'
})
export class DiagnoseService {
private url = environment.apiUrl + 'diagnose/'
  constructor(private http: HttpClient) { }

  public addDiagnoseToCard(appointmentId: number, medicalCardId: number, diagnose: string): Observable<MedicalCard> {
    return this.http.post<MedicalCard>(this.url + `create/${appointmentId}`, {medicalCardId, diagnose})
  }

  public updateDiagnose(diagnoseId: number, diagnose: string): Observable<MedicalCard> {
    return this.http.patch<MedicalCard>(this.url + `update/${diagnoseId}`, {diagnose})
  }

  public deleteDiagnose(diagnoseId: number): Observable<any> {
    return this.http.delete(this.url + `delete/${diagnoseId}`)
  }
}
