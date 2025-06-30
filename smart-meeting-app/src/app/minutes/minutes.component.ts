import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-minutes',
  standalone: true,
  imports: [CommonModule], 
  templateUrl: './minutes.component.html',
  styleUrl: './minutes.component.scss'
})
export class MinutesComponent {
  minutes = [
    {
      title: 'Sprint Planning',
      date: new Date('2025-06-25'),
      attendees: ['Ali', 'Layal', 'Hadi'],
      recorded: true
    },
    {
      title: 'Client Review',
      date: new Date('2025-06-20'),
      attendees: ['Maya', 'Sam', 'Nadim'],
      recorded: false
    }
  ];
}
