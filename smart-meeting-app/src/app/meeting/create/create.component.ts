import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

interface BookingOption {
  id: number;
  roomName: string;
  startTime: string;
  endTime: string;
}

@Component({
  selector: 'app-create-meeting',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.scss']
})
export class CreateComponent implements OnInit {
  meetingForm: FormGroup;
  bookings: BookingOption[] = [];
  successMessage = '';
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router
  ) {
    this.meetingForm = this.fb.group({
      bookingID: [null, Validators.required],
      title: ['', Validators.required],
      agenda: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadBookings();
  }


  loadBookings(): void {
    this.http.get<BookingOption[]>('/api/Bookings').subscribe({
      next: (data) => (this.bookings = data),
      error: () => {
        this.errorMessage = 'Failed to load bookings.';
        this.clearErrorAfterDelay();
      }
    });
  }
 goToDashboard(): void {
    this.router.navigate(['/admin']);
  }
  submit(): void {
    if (this.meetingForm.invalid) return;

    this.http.post('/api/Meetings', this.meetingForm.value).subscribe({
      next: () => {
        this.successMessage = 'Meeting created successfully!';
        setTimeout(() => this.router.navigate(['/meetings/active']), 1500);
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to create meeting.';
        this.clearErrorAfterDelay();
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/meetings/active']);
  }

  private clearErrorAfterDelay(): void {
    setTimeout(() => (this.errorMessage = ''), 3000);
  }
}
