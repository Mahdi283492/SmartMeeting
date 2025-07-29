import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl = 'http://localhost:5268/api/users';

  constructor(private http: HttpClient) {}


  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl);
  }


  getUserById(id: number): Observable<User> {
    return this.http.get<User>(`${this.baseUrl}/${id}`);
  }

  createUser(user: any): Observable<any> {
    return this.http.post(this.baseUrl, user);
  }


  updateUser(id: number, payload: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/${id}`, payload);
  }

  deleteUser(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}
