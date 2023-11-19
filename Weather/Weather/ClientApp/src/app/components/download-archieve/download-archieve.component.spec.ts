import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DownloadArchieveComponent } from './download-archieve.component';

describe('DownloadArchieveComponent', () => {
  let component: DownloadArchieveComponent;
  let fixture: ComponentFixture<DownloadArchieveComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DownloadArchieveComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DownloadArchieveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
