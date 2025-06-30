import { Injectable } from '@angular/core';
import { HttpClient }   from '@angular/common/http';
import { Router }       from '@angular/router';
import { tap }          from 'rxjs/operators';
import { Observable }   from 'rxjs';

interface LoginResponse {
  token: string;
  roles?: string[];
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = '/api/Auth';

  constructor(private http: HttpClient, private router: Router) {}
 private get storage(): Storage | null {
    return typeof window !== 'undefined' ? window.localStorage : null;
  }
  login(email: string, password: string): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${this.apiUrl}/login`, { email, password })
      .pipe(
        tap(res => {
          localStorage.setItem('token', res.token);

        })
      );
  }

  logout() {
    localStorage.removeItem('jwt');
    this.router.navigate(['/login']);
  }

get token(): string | null {
  return this.storage?.getItem('token') ?? null;
}

  get isLoggedIn(): boolean {
    return !!this.token;
  }
}
