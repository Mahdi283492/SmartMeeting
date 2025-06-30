import { TestBed }    from '@angular/core/testing';
import { Router }     from '@angular/router';
import { AuthGuard }  from './auth.guard';
import { AuthService } from './auth.service';

describe('AuthGuard', () => {
  let guard: AuthGuard;
  let authService: jasmine.SpyObj<AuthService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(() => {

    authService = jasmine.createSpyObj('AuthService', [], {});
    router      = jasmine.createSpyObj('Router', ['createUrlTree']);

    TestBed.configureTestingModule({
      providers: [
        AuthGuard,
        { provide: AuthService, useValue: authService },
        { provide: Router,      useValue: router }
      ]
    });
    guard = TestBed.inject(AuthGuard);
  });

  it('allows activation when logged in', () => {

    Object.defineProperty(authService, 'isLoggedIn', { get: () => true });
    expect(guard.canActivate()).toBeTrue();
  });

  it('redirects to /login when not logged in', () => {
    Object.defineProperty(authService, 'isLoggedIn', { get: () => false });
    const fakeTree = {} as any;
    router.createUrlTree.and.returnValue(fakeTree);

    const result = guard.canActivate();
    expect(router.createUrlTree).toHaveBeenCalledWith(['/login']);
    expect(result).toBe(fakeTree);
  });
});
