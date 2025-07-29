import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../core/user.service';

@Component({
  selector: 'app-user-edit',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.scss']
})
export class UserEditComponent implements OnInit {
  userId!: number;
  user: any = { name: '', email: '', role: '' };
  loading = true;
  saving = false;
  error: string | null = null;
  success: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.userId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadUser();
  }

  loadUser(): void {
    this.userService.getUserById(this.userId).subscribe({
      next: (data) => {
        this.user = {
          name: data.name,
          email: data.email,
          role: data.roles?.[0] || 'User'
        };
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load user details';
        this.loading = false;
      }
    });
  }

  save(): void {
    this.saving = true;
    const payload = {
      name: this.user.name,
      email: this.user.email,
      role: this.user.role
    };
    this.userService.updateUser(this.userId, payload).subscribe({
      next: () => {
        this.success = 'User updated successfully';
        this.saving = false;
        setTimeout(() => this.goBack(), 1500);
      },
      error: () => {
        this.error = 'Failed to update user';
        this.saving = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/admin/user-list']);
  }
}
