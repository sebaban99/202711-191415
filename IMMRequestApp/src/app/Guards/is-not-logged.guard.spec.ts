import { TestBed } from '@angular/core/testing';

import { IsNotLoggedGuard } from './is-not-logged.guard';

describe('IsNotLoggedGuardGuard', () => {
  let guard: IsNotLoggedGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(IsNotLoggedGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
