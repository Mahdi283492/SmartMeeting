import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface BookingsPerMonth {
  year: number;
  month: number;
  count: number;
}

export interface ReportSummary {
  totalMeetings: number;
  totalRooms: number;
  totalBookings: number;
  totalUsers: number;
  mostUsedRoom: string;
  bookingsPerMonth: BookingsPerMonth[];
}

@Injectable({ providedIn: 'root' })
export class ReportsService {
  private apiUrl = '/api/Reports/summary';

  constructor(private http: HttpClient) {}

  getSummary(): Observable<ReportSummary> {
    return this.http.get<ReportSummary>(this.apiUrl);
  }
}
