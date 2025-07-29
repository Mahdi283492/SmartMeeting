import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ActionItem {
  id: number;
  description: string;
  dueDate: string;
  status: string;
  minutesId: number;
   meetingTitle: string;
}

export interface CreateActionItemDto {
  minutesID: number;
  assignedTo: number;
  description: string;
  dueDate: string;
  status: string;
}

@Injectable({ providedIn: 'root' })
export class ActionItemService {
  private apiUrl = '/api/ActionItems';

  constructor(private http: HttpClient) {}

  getMine(): Observable<ActionItem[]> {
    return this.http.get<ActionItem[]>(`${this.apiUrl}/my`);
  }

  create(dto: CreateActionItemDto): Observable<any> {
    return this.http.post(this.apiUrl, dto);
  }

  update(id: number, dto: CreateActionItemDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
