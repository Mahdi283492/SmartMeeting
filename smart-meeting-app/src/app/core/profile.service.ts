import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface UserProfile {
  id: number;
  name: string;
  email: string;
  roles: string[];
}

export interface UpdateProfileDto {
  name: string;
  newPassword?: string;
}

@Injectable({ providedIn: 'root' })
export class ProfileService {
  private apiUrl = '/api/Users';

  constructor(private http: HttpClient) {}

  getProfile(): Observable<UserProfile> {
    return this.http.get<UserProfile>(`${this.apiUrl}/me`);
  }

  updateProfile(dto: UpdateProfileDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/me`, dto);
  }
}
