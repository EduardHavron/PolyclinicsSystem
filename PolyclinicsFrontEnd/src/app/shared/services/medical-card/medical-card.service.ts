import {Injectable} from '@angular/core';
import {environment} from "../../../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {MedicalCard} from "../../models/medical-card/medical-card";

@Injectable({
  providedIn: 'root'
})
export class MedicalCardService {
private url = environment.apiUrl + 'medCard/'
  constructor(private http: HttpClient) { }

  public getMedCard(userId: string, includeDiagnose: boolean): Observable<MedicalCard> {
    return this.http.get<MedicalCard>(this.url + `get/${userId}?includeDiagnoses=${includeDiagnose}`)
  }

  public updateMedCard(medicalCardId: number, medicalCard: MedicalCard): Observable<MedicalCard> {
    return this.http.patch<MedicalCard>(this.url + `update/${medicalCardId}`, medicalCard)
  }
}
