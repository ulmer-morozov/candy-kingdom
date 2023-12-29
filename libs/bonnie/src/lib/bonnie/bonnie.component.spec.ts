import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BonnieComponent } from './bonnie.component';

describe('BonnieComponent', () => {
  let component: BonnieComponent;
  let fixture: ComponentFixture<BonnieComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BonnieComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(BonnieComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
