import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Meeting {
  id: number;
  title: string;
  start: string;
  roomName: string;
}

@Injectable({ providedIn: 'root' })
export class MeetingService {
  private apiUrl = '/api/Meetings';

  constructor(private http: HttpClient) {}

  getUpcoming(): Observable<Meeting[]> {
    return this.http.get<Meeting[]>(`${this.apiUrl}/upcoming`);
  }
}
