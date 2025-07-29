import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../../models/user.model';
import { UserService } from '../../core/user.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
declare var bootstrap: any;

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {
  users: (User & { displayRole?: string })[] = [];
  newUser: any = {};
  loading = false;
  successMessage: string | null = null;
  error: string | null = null;
editUser: any = {};
editingUserId: number | null = null;
  deleteTargetId: number | null = null;
  deleting = false;

  constructor(private userService: UserService, private router: Router) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  goToDashboard(): void {
    this.router.navigate(['/admin']);
  }

  private showSuccess(message: string): void {
    this.successMessage = message;
    setTimeout(() => (this.successMessage = null), 3000);
  }

  private showError(message: string): void {
    this.error = message;
    setTimeout(() => (this.error = null), 3000);
  }

  loadUsers(): void {
    this.loading = true;
    this.userService.getAllUsers().subscribe({
      next: (data: User[]) => {
        this.users = data.map(u => ({
          ...u,
          displayRole: u.roles && u.roles.length ? u.roles[0] : 'User'
        }));
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.showError('Error loading users');
        this.loading = false;
      }
    });
  }

  submitAddUser(): void {
    const payload = {
      name: this.newUser.name,
      email: this.newUser.email,
      password: this.newUser.password,
      role: this.newUser.role
    };

    this.userService.createUser(payload).subscribe({
      next: () => {
        this.showSuccess('User created successfully');
        this.loadUsers();
        this.newUser = {};
      },
      error: (err) => {
        console.error(err);
        this.showError(err.error?.message || 'Create failed');
      }
    });
  }

  openDeleteModal(id: number): void {
    this.deleteTargetId = id;
  }

  confirmDelete(): void {
    if (this.deleteTargetId == null || this.deleting) return;

    this.deleting = true;
    this.userService.deleteUser(this.deleteTargetId).subscribe({
      next: () => {
        this.showSuccess('User deleted successfully');
        this.loadUsers();
        this.deleteTargetId = null;
        this.deleting = false;

        const modalElement = document.getElementById('deleteUserModal');
        const modal = bootstrap.Modal.getInstance(modalElement);
        modal?.hide();
      },
      error: (err) => {
        console.error(err);
        this.showError('Delete failed');
        this.deleting = false;
      }
    });
  }
}
