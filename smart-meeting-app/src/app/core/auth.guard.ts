import { Injectable } from '@angular/core';
import {
  CanActivate,
  Router,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private auth: AuthService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean | UrlTree {
    const isLoggedIn = this.auth.isLoggedIn;
    const role = this.auth.userRole;


    if (!isLoggedIn) {
      return this.router.createUrlTree(['/login']);
    }


    const isAdminRoute = state.url.startsWith('/admin');
    if (isAdminRoute && role !== 'Admin') {
      return this.router.createUrlTree(['/dashboard']);
    }

    return true;
  }
}
