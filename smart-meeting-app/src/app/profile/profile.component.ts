import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ProfileService, UserProfile } from '../core/profile.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  profile: UserProfile | null = null;
  form!: FormGroup;
  message = '';
  error = '';

  constructor(private fb: FormBuilder, private profileService: ProfileService, private router: Router) {}

  ngOnInit(): void {
    this.loadProfile();
  }
goToDashboard(): void {
    this.router.navigate(['/admin']);
  }
  loadProfile(): void {
    this.profileService.getProfile().subscribe({
      next: (data) => {
        this.profile = data;
        this.form = this.fb.group({
          name: [data.name, Validators.required],
          newPassword: ['']
        });
      },
      error: (err) => {
        this.error = err.error?.message || 'Failed to load profile';
      }
    });
  }

  save(): void {
    if (this.form.invalid) return;
    this.profileService.updateProfile(this.form.value).subscribe({
      next: () => {
        this.message = 'Profile updated successfully';
        this.error = '';
      },
      error: (err) => {
        this.error = err.error?.message || 'Update failed';
        this.message = '';
      }
    });
  }
}
