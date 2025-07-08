import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RoomService, Room } from '../core/room.service';

@Component({
  selector: 'app-booking',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './booking.component.html',
  styleUrl: './booking.component.scss',
})
export class BookingComponent implements OnInit {
  bookingForm: FormGroup;
  successMessage = '';
  errorMessage = '';
  rooms: Room[] = [];

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private roomService: RoomService
  ) {
    this.bookingForm = this.fb.group({
      roomID: [null, Validators.required],
      startTime: ['', Validators.required],
      durationMinutes: [30, [Validators.required, Validators.min(1)]],
      status: ['Pending', Validators.required],
    });
  }

  ngOnInit(): void {
    this.loadRooms();
  }

  loadRooms(): void {
    this.roomService.getAllRooms().subscribe({
      next: (res) => (this.rooms = res),
      error: () => (this.errorMessage = 'Failed to load rooms.'),
    });
  }

  bookMeeting(): void {
    if (this.bookingForm.invalid) return;

    const { startTime, durationMinutes, roomID, status } = this.bookingForm.value;

    const start = new Date(startTime);
    const end = new Date(start.getTime() + durationMinutes * 60000);

    const payload = {
      roomID,
      startTime: start,
      endTime: end,
      status,
    };

    this.http.post('/api/Bookings/book', payload).subscribe({
      next: () => {
        this.successMessage = 'Room booked successfully!';
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Booking failed.';
      },
    });
  }
}
