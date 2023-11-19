import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowArchieveComponent } from './show-archieve.component';

describe('ShowArchieveComponent', () => {
  let component: ShowArchieveComponent;
  let fixture: ComponentFixture<ShowArchieveComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowArchieveComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShowArchieveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
