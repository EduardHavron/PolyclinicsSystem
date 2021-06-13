import {Component, Input, OnInit} from '@angular/core';
import {MedicalCardService} from "../../../shared/services/medical-card/medical-card.service";
import {MedicalCard} from "../../../shared/models/medical-card/medical-card";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Patient} from "../../../shared/models/patient/patient";
import {Gender} from "../../../shared/enums/gender";

@Component({
  selector: 'app-view-medical-card',
  templateUrl: './view-medical-card.component.html',
  styleUrls: ['./view-medical-card.component.css']
})
export class ViewMedicalCardComponent implements OnInit {
  @Input()
   medicalCard = new MedicalCard()

  @Input()
   patient = new Patient()

  public genderView = ''
  constructor(private medCardService: MedicalCardService,
              private snackBar: MatSnackBar) {
    this.genderView = this.medicalCard.gender === Gender.Male ? "Male" : "Female"

  }

  public formatDate(dateString: string) : string {
    return new Date(dateString).toLocaleString().substring(0, 17)
  }

  ngOnInit(): void {
  }

}
