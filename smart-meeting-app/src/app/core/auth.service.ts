import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

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
          this.storage?.setItem('token', res.token);
        })
      );
  }

  logout(): void {
    this.storage?.removeItem('token');
    this.router.navigate(['/login']);
  }

  get token(): string | null {
    return this.storage?.getItem('token') ?? null;
  }

get isLoggedIn(): boolean {
  const token = this.token;
  if (!token) return false;

  try {
    
    const payload = JSON.parse(atob(token.split('.')[1]));
    return !!payload;
  } catch {
    return false;
  }
}


  get userRole(): string | null {
    const token = this.token;
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || null;
    } catch {
      return null;
    }
  }


  get userEmail(): string | null {
    const token = this.token;
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] || null;
    } catch {
      return null;
    }
  }
}
