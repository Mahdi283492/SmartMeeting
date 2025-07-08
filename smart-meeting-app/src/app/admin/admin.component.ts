import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {
  summary: any = null;
  error = '';
  loading = true;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get('/api/Admin/summary').subscribe({
      next: data => {
        this.summary = data;
        this.loading = false;
      },
      error: err => {
        this.error = err.error?.message || 'Failed to load dashboard summary';
        this.loading = false;
      }
    });
  }
}
