import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BonnieCmsComponent } from './bonnie-cms.component';

describe('BonnieCmsComponent', () => {
  let component: BonnieCmsComponent;
  let fixture: ComponentFixture<BonnieCmsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BonnieCmsComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(BonnieCmsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
