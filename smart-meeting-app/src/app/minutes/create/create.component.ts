import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MinutesService } from '../../core/minutes.service';

@Component({
  selector: 'app-minutes-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.scss']
})
export class CreateComponent implements OnInit {
  minutesForm: FormGroup;
  meetings: any[] = [];
  successMessage = '';
  errorMessage = '';

  constructor(private fb: FormBuilder, private minutesService: MinutesService, private router: Router) {
    this.minutesForm = this.fb.group({
      meetingID: ['', Validators.required],
      discussionPoints: ['', Validators.required],
      decisions: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadMeetings();
  }
 goToDashboard(): void {
    this.router.navigate(['/admin']);
  }
  loadMeetings(): void {
    this.minutesService.getMeetings().subscribe({
      next: (data) => (this.meetings = data),
      error: () => (this.errorMessage = 'Failed to load meetings.')
    });
  }

  submit(): void {
    if (this.minutesForm.invalid) return;
    this.minutesService.create(this.minutesForm.value).subscribe({
      next: () => {
        this.successMessage = 'Minutes created successfully!';
        setTimeout(() => this.router.navigate(['/minutes/list']), 1500);
      },
      error: (err) => (this.errorMessage = err.error?.message || 'Failed to create minutes.')
    });
  }

  cancel(): void {
    this.router.navigate(['/minutes/list']);
  }
}
