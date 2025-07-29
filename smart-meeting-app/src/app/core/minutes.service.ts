import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Minutes {
  id: number;
  meetingTitle: string;
  discussionPoints: string;
  decisions: string;
  startTime: string;
  roomName: string;
}

export interface CreateMinutesDto {
  meetingID: number;
  discussionPoints: string;
  decisions: string;
}

@Injectable({ providedIn: 'root' })
export class MinutesService {
  private apiUrl = '/api/Minutes';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Minutes[]> {
    return this.http.get<Minutes[]>(this.apiUrl);  // Still returns raw minutes
  }

  getMine(): Observable<Minutes[]> {
    return this.http.get<Minutes[]>(`${this.apiUrl}/mine`);
  }

  getDetails(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${id}/details`);
  }

  create(dto: CreateMinutesDto): Observable<any> {
    return this.http.post(this.apiUrl, dto);
  }
createActionItem(payload: any) {
  return this.http.post('/api/ActionItems', payload);
}

deleteActionItem(id: number) {
  return this.http.delete(`/api/ActionItems/${id}`);
}

  update(id: number, dto: CreateMinutesDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, dto);
  }

  getMeetings(): Observable<any[]> {
    return this.http.get<any[]>('/api/Meetings');
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
