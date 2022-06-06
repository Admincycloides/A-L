import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewtimesheetComponent } from './reviewtimesheet.component';

describe('ReviewtimesheetComponent', () => {
  let component: ReviewtimesheetComponent;
  let fixture: ComponentFixture<ReviewtimesheetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReviewtimesheetComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReviewtimesheetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
