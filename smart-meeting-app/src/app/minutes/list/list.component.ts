import { Component, OnInit } from '@angular/core';
import { MinutesService, Minutes } from '../../core/minutes.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-minutes-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class MinutesListComponent implements OnInit {
  minutes: Minutes[] = [];
  loading = false;
  error: string | null = null;

  constructor(private minutesService: MinutesService, private router: Router) {}

  ngOnInit(): void {
    this.loadMinutes();
  }
 goToDashboard(): void {
    this.router.navigate(['/admin']);
  }
  loadMinutes(): void {
    this.loading = true;
    this.minutesService.getAll().subscribe({
      next: (data) => {
        this.minutes = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load minutes.';
        this.loading = false;
      }
    });
  }

viewDetails(id: number): void {
  this.router.navigate(['/minutes', id, 'details']);
}


  deleteMinutes(id: number): void {
    if (!confirm('Are you sure you want to delete this record?')) return;
    this.minutesService.delete(id).subscribe({
      next: () => this.loadMinutes(),
      error: () => (this.error = 'Failed to delete minutes')
    });
  }
}
