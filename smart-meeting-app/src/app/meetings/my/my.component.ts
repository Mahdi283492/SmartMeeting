import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MeetingService, Meeting } from '../../core/meeting.service';

@Component({
  selector: 'app-my-meetings',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './my.component.html',
  styleUrls: ['./my.component.scss']
})
export class MyComponent implements OnInit {
  meetings: Meeting[] = [];
  loading = false;
  error: string | null = null;

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
      error: () => {
        this.error = 'Failed to load your meetings.';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/dashboard']);
  }
}
