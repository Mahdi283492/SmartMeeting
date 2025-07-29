import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

interface Booking {
  id: number;
  startTime: string;
  endTime: string;
  roomID: number;
  status: string;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  bookings: Booking[] = [];
  error = '';

  constructor(private http: HttpClient, private router: Router) {}
logout(): void {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
  ngOnInit() {
    this.http.get<Booking[]>('/api/Bookings/my').subscribe({
      next: data => this.bookings = data,
      error: err => {
        console.error('Failed to load bookings', err);
        this.error = err.error?.title || err.message || 'Something went wrong';
      }
    });
  }
}
