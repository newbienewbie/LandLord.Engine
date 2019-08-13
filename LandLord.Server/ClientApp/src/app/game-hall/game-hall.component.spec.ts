import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GameHallComponent } from './game-hall.component';

describe('GameHallComponent', () => {
  let component: GameHallComponent;
  let fixture: ComponentFixture<GameHallComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GameHallComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GameHallComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
