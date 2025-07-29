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

export interface CreateRoomDto {
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

  getRoomById(id: number): Observable<Room> {
    return this.http.get<Room>(`${this.apiUrl}/${id}`);
  }

  createRoom(room: CreateRoomDto): Observable<Room> {
    return this.http.post<Room>(this.apiUrl, room);
  }

  updateRoom(id: number, room: CreateRoomDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, room);
  }

  deleteRoom(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
