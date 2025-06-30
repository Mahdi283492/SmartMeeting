import { Component, OnInit }     from '@angular/core';
import { CommonModule }           from '@angular/common';
import { MeetingService, Meeting } from '../core/meeting.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [ CommonModule ],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
[x: string]: any;
  meetings: Meeting[] = [];

  constructor(private meetingsSvc: MeetingService) {}

  ngOnInit() {
    this.meetingsSvc.getUpcoming().subscribe({
      next: data => this.meetings = data,
      error: err => {
  console.error('Failed to load meetings', err);
  this['error'] = err.error?.title || err.message || 'Something went wrong';
}
    });
  }
}
