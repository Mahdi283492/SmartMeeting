import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Room {
  id: number;
  name: string;
  capacity: number;
  location: string;
  features: string;
}

@Injectable({ providedIn: 'root' })
export class RoomService {
  private apiUrl = '/api/Rooms';

  constructor(private http: HttpClient) {}

  getAllRooms(): Observable<Room[]> {
    return this.http.get<Room[]>(this.apiUrl);
  }
}
