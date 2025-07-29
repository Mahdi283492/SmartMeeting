import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../core/auth.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    RouterModule
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  email = '';
  password = '';
  error: string | null = null;

  constructor(private auth: AuthService, private http: HttpClient, private router: Router) {}

  onSubmit(): void {
    this.error = null;

    this.auth.login(this.email, this.password).subscribe({
      next: () => {
        this.http.get<any>('/api/Auth/me').subscribe({
          next: user => {
            if (user.roles.includes('Admin')) {
              this.router.navigate(['/admin']);
            } else {
              this.router.navigate(['/dashboard']);
            }
          },
          error: () => {
            this.router.navigate(['/dashboard']);
          }
        });
      },
      error: err => {
        if (err.status === 401) {
          this.error = 'Invalid email or password!';
        } else {
          this.error = 'Login failed. Please try again later.';
        }
      }
    });
  }
}
