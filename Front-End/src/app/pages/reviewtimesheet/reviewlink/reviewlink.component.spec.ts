import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewlinkComponent } from './reviewlink.component';

describe('ReviewlinkComponent', () => {
  let component: ReviewlinkComponent;
  let fixture: ComponentFixture<ReviewlinkComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReviewlinkComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReviewlinkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
