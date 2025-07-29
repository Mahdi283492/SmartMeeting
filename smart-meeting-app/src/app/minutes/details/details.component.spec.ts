import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MinutesDetailsComponent } from './details.component';

describe('DetailsComponent', () => {
  let component: MinutesDetailsComponent;
  let fixture: ComponentFixture<MinutesDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MinutesDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MinutesDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
