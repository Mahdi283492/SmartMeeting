import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MinutesService } from '../../core/minutes.service';
import { HttpClient } from '@angular/common/http';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

@Component({
  selector: 'app-minutes-details',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './details.component.html',
  styleUrls: ['./details.component.scss']
})
export class MinutesDetailsComponent implements OnInit {
  minutesId!: number;
  minutesDetails: any = null;
  users: any[] = [];
  error: string | null = null;

  newActionItem = {
    description: '',
    dueDate: '',
    assignedTo: null,
    status: 'Pending'
  };

  constructor(
    private route: ActivatedRoute,
    private minutesService: MinutesService,
    private http: HttpClient,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.minutesId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadMinutesDetails();
    this.loadUsers();
  }

  goToDashboard(): void {
    this.router.navigate(['/admin']);
  }

  loadMinutesDetails(): void {
    this.minutesService.getDetails(this.minutesId).subscribe({
      next: (data) => (this.minutesDetails = data),
      error: () => (this.error = 'Failed to load details')
    });
  }

  loadUsers(): void {
    this.http.get<any[]>('/api/Users').subscribe({
      next: (data) => (this.users = data),
      error: () => console.error('Failed to load users')
    });
  }

  addActionItem(): void {
    if (!this.newActionItem.description || !this.newActionItem.dueDate || !this.newActionItem.assignedTo) return;

    const payload = {
      minutesID: this.minutesId,
      description: this.newActionItem.description,
      dueDate: this.newActionItem.dueDate,
      assignedTo: this.newActionItem.assignedTo,
      status: this.newActionItem.status
    };

    this.minutesService.createActionItem(payload).subscribe({
      next: () => {
        this.loadMinutesDetails();
        this.newActionItem = { description: '', dueDate: '', assignedTo: null, status: 'Pending' };
      },
      error: () => alert('Failed to add action item')
    });
  }

  deleteActionItem(id: number): void {
    if (!confirm('Delete this action item?')) return;
    this.minutesService.deleteActionItem(id).subscribe({
      next: () => this.loadMinutesDetails(),
      error: () => alert('Failed to delete action item')
    });
  }

  downloadPDF(): void {
    if (!this.minutesDetails) return;

    const doc = new jsPDF();

    doc.setFontSize(14);
    doc.text('Minutes of Meeting', 14, 20);

    doc.setFontSize(11);
    doc.text(`Meeting: ${this.minutesDetails.meetingTitle}`, 14, 30);
    doc.text(`Start: ${new Date(this.minutesDetails.startTime).toLocaleString()}`, 14, 38);
    doc.text(`Room: ${this.minutesDetails.roomName}`, 14, 46);

    doc.setFontSize(12);
    doc.text('Discussion Points:', 14, 60);
    doc.setFontSize(10);
    doc.text(this.minutesDetails.discussionPoints || '-', 14, 68);

    doc.setFontSize(12);
    doc.text('Decisions:', 14, 85);
    doc.setFontSize(10);
    doc.text(this.minutesDetails.decisions || '-', 14, 93);

    if (this.minutesDetails.actionItems?.length) {
      autoTable(doc, {
        head: [['Description', 'Assigned To', 'Due Date', 'Status']],
        body: this.minutesDetails.actionItems.map((ai: any) => [
          ai.description,
          ai.assignedUser,
          new Date(ai.dueDate).toLocaleDateString(),
          ai.status
        ]),
        startY: 110
      });
    }

    doc.save(`Minutes_${this.minutesDetails.meetingTitle}.pdf`);
  }
}
