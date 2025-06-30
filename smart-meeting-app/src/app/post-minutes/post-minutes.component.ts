import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-post-minutes',
  standalone: true,
  imports: [CommonModule],  
  templateUrl: './post-minutes.component.html',
  styleUrl: './post-minutes.component.scss'
})
export class PostMinutesComponent {
  reviewedMinutes = [
    {
      title: 'Team Sync - Week 25',
      date: new Date('2025-06-25'),
      summary: 'Reviewed current sprint goals and blockers. Agreed on next actions.',
      status: 'Completed'
    },
    {
      title: 'Marketing Strategy',
      date: new Date('2025-06-22'),
      summary: 'Initial discussion on campaign direction. Final approval pending.',
      status: 'Pending'
    }
  ];
}
