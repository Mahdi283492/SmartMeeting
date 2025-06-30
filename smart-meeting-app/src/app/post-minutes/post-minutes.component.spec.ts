import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostMinutesComponent } from './post-minutes.component';

describe('PostMinutesComponent', () => {
  let component: PostMinutesComponent;
  let fixture: ComponentFixture<PostMinutesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PostMinutesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PostMinutesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
