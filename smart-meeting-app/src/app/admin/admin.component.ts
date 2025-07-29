import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ReportsService, ReportSummary } from '../core/reports.service';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, RouterModule, MatCardModule, MatProgressSpinnerModule],
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {
  summary: ReportSummary | null = null;
  error = '';
  loading = true;

  constructor(private reportsService: ReportsService, private router: Router) {}

  ngOnInit(): void {
    this.loadSummary();
  }
  logout(): void {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
  loadSummary(): void {
    this.loading = true;
    this.reportsService.getSummary().subscribe({
      next: (data) => {
        this.summary = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = err.error?.message || 'Failed to load dashboard summary';
        this.loading = false;
      }
    });
  }
}
