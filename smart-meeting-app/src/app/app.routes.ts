import { Routes } from '@angular/router';

import { LoginComponent } from './auth/login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { BookingComponent } from './booking/booking.component';
import { ActiveComponent } from './meeting/active/active.component';
import { MinutesComponent } from './minutes/minutes.component';
import { AdminComponent } from './admin/admin.component';
import { AuthGuard } from './core/auth.guard';
import { RoomManagementComponent } from './admin/room-management/room-management.component';
import { CreateComponent } from './meeting/create/create.component';
import { MyComponent } from './meetings/my/my.component';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },

  { path: 'login', component: LoginComponent },

  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuard],
  },
  { path: 'booking', component: BookingComponent, canActivate: [AuthGuard] },
  {
    path: 'meeting/active',
    component: ActiveComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'meetings/create',
    component: CreateComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'profile',
    loadComponent: () =>
      import('./profile/profile.component').then((m) => m.ProfileComponent),
    canActivate: [AuthGuard],
  },
  { path: 'meetings/my', component: MyComponent, canActivate: [AuthGuard] },

  { path: 'minutes', component: MinutesComponent, canActivate: [AuthGuard] },

  { path: 'admin', component: AdminComponent, canActivate: [AuthGuard] },
  {
    path: 'post-minutes',
    loadComponent: () =>
      import('./post-minutes/post-minutes.component').then(
        (m) => m.PostMinutesComponent
      ),
    canActivate: [AuthGuard],
  },
  {
    path: 'my-bookings',
    loadComponent: () =>
      import('./my-bookings/my-bookings.component').then(
        (m) => m.MyBookingsComponent
      ),
  },
  {
  path: 'admin/user-edit/:id',
  loadComponent: () => import('./admin/user-edit/user-edit.component')
    .then(m => m.UserEditComponent)
}
,
  {
    path: 'action-items/my',
    loadComponent: () =>
      import('./action-items/my/my.component').then(
        (m) => m.MyActionItemsComponent
      ),
  },
  {
    path: 'admin/rooms',
    component: RoomManagementComponent,
  },
  {
    path: 'admin/room-management',
    loadComponent: () =>
      import('./admin/room-management/room-management.component').then(
        (m) => m.RoomManagementComponent
      ),
  },
  {
    path: 'admin/user-list',
    loadComponent: () =>
      import('./admin/user-list/user-list.component').then(
        (m) => m.UserListComponent
      ),
  },
  {
    path: 'minutes/create',
    loadComponent: () =>
      import('./minutes/create/create.component').then(
        (m) => m.CreateComponent
      ),
    canActivate: [AuthGuard],
  },
  {
    path: 'minutes/list',
    loadComponent: () =>
      import('./minutes/list/list.component').then(
        (m) => m.MinutesListComponent
      ),
    canActivate: [AuthGuard],
  },
  {
    path: 'minutes/:id/details',
    loadComponent: () =>
      import('./minutes/details/details.component').then(
        (m) => m.MinutesDetailsComponent
      ),
    canActivate: [AuthGuard],
  },
  { path: '**', redirectTo: 'login' },
];
