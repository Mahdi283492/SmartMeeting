import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MinutesListComponent } from './list.component';

describe('ListComponent', () => {
  let component: MinutesListComponent;
  let fixture: ComponentFixture<MinutesListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MinutesListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MinutesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
