import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { PLATFORM_ID } from '@angular/core';

@Component({
  selector: 'app-my-bookings',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './my-bookings.component.html',
  styleUrls: ['./my-bookings.component.scss']
})
export class MyBookingsComponent implements OnInit {
  bookings: any[] = [];
  loading = true;
  error = '';
  bookingToCancel: number | null = null;

  constructor(
    private http: HttpClient,
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      this.loadBookings();
    }
  }

  loadBookings(): void {
    this.loading = true;
    this.http.get<any[]>('/api/Bookings/my').subscribe({
      next: data => {
        this.bookings = data;
        this.loading = false;
      },
      error: err => {
        this.error = err.error?.message || 'Failed to load bookings';
        this.loading = false;
      }
    });
  }

  showCancelModal(id: number): void {
    this.bookingToCancel = id;
  }

  cancelCancel(): void {
    this.bookingToCancel = null;
  }

  confirmCancel(): void {
    if (!this.bookingToCancel) return;

    const id = this.bookingToCancel;
    this.http.delete(`/api/Bookings/${id}`).subscribe({
      next: () => {
        this.bookings = this.bookings.filter(b => b.id !== id);
        this.bookingToCancel = null;
      },
      error: err => {
        alert(err.error?.message || 'Failed to cancel booking.');
        this.bookingToCancel = null;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/dashboard']);
  }
}
