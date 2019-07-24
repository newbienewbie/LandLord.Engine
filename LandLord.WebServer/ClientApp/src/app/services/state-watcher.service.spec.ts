import { TestBed, inject } from '@angular/core/testing';

import { RoomStateWatcherService } from './state-watcher.service';

describe('StateWatcherService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RoomStateWatcherService]
    });
  });

  it('should be created', inject([RoomStateWatcherService], (service: RoomStateWatcherService) => {
    expect(service).toBeTruthy();
  }));
});
