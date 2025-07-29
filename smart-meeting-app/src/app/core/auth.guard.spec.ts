import { TestBed } from '@angular/core/testing';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthGuard } from './auth.guard';
import { AuthService } from './auth.service';

describe('AuthGuard', () => {
  let guard: AuthGuard;
  let authService: jasmine.SpyObj<AuthService>;
  let router: jasmine.SpyObj<Router>;
  let mockRoute: ActivatedRouteSnapshot;
  let mockState: RouterStateSnapshot;

  beforeEach(() => {
    authService = jasmine.createSpyObj('AuthService', [], {});
    router = jasmine.createSpyObj('Router', ['createUrlTree']);

    TestBed.configureTestingModule({
      providers: [
        AuthGuard,
        { provide: AuthService, useValue: authService },
        { provide: Router, useValue: router }
      ]
    });

    guard = TestBed.inject(AuthGuard);
    mockRoute = {} as ActivatedRouteSnapshot;
    mockState = {} as RouterStateSnapshot;
  });

  it('allows activation when logged in and is admin', () => {
    Object.defineProperty(authService, 'isLoggedIn', { get: () => true });
    Object.defineProperty(authService, 'userRole', { get: () => 'Admin' });

    const result = guard.canActivate(mockRoute, mockState);
    expect(result).toBeTrue();
  });

  it('redirects to /login when not logged in', () => {
    Object.defineProperty(authService, 'isLoggedIn', { get: () => false });

    const fakeTree = {} as any;
    router.createUrlTree.and.returnValue(fakeTree);

    const result = guard.canActivate(mockRoute, mockState);
    expect(result).toBe(fakeTree);
  });

  it('redirects to /dashboard when logged in but not admin', () => {
    Object.defineProperty(authService, 'isLoggedIn', { get: () => true });
    Object.defineProperty(authService, 'userRole', { get: () => 'User' });

    const dashboardTree = {} as any;
    router.createUrlTree.and.returnValue(dashboardTree);

    const result = guard.canActivate(mockRoute, mockState);
    expect(router.createUrlTree).toHaveBeenCalledWith(['/dashboard']);
    expect(result).toBe(dashboardTree);
  });
});
