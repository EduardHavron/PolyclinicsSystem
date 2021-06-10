import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewMedicalCardComponent } from './view-medical-card.component';

describe('ViewMedicalCardComponent', () => {
  let component: ViewMedicalCardComponent;
  let fixture: ComponentFixture<ViewMedicalCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ViewMedicalCardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewMedicalCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
