import {ComponentFixture, TestBed} from '@angular/core/testing';

import {HomePageDoctorComponent} from './home-page-doctor.component';

describe('HomePageComponent', () => {
  let component: HomePageDoctorComponent;
  let fixture: ComponentFixture<HomePageDoctorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HomePageDoctorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HomePageDoctorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
