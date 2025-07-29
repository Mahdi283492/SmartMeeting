import { Component, OnInit } from '@angular/core';
import { MeetingService, Meeting } from '../../core/meeting.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-active',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './active.component.html',
  styleUrls: ['./active.component.scss']
})
export class ActiveComponent implements OnInit {
  meetings: Meeting[] = [];
  loading = false;
  error = '';

  constructor(private meetingService: MeetingService, private router: Router) {}

  ngOnInit(): void {
    this.loadMeetings();
  }

  loadMeetings(): void {
    this.loading = true;
    this.meetingService.getUpcoming().subscribe({
      next: (data) => {
        this.meetings = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = err.error?.message || 'Failed to load meetings';
        this.loading = false;
      }
    });
  }

  goToCreate(): void {
    this.router.navigate(['/meetings/create']);
  }

  goBack(): void {
    this.router.navigate(['/dashboard']);
  }
}
