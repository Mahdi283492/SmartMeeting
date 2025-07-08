import { Routes } from '@angular/router';

import { LoginComponent } from './auth/login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { BookingComponent } from './booking/booking.component';
import { ActiveComponent } from './meeting/active/active.component';
import { MinutesComponent } from './minutes/minutes.component';
import { HistoryComponent } from './history/history.component';
import { AdminComponent } from './admin/admin.component';
import { AuthGuard } from './core/auth.guard';

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
  { path: 'minutes', component: MinutesComponent, canActivate: [AuthGuard] },
  { path: 'history', component: HistoryComponent, canActivate: [AuthGuard] },
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
  loadComponent: () => import('./my-bookings/my-bookings.component').then(m => m.MyBookingsComponent)
},

  { path: '**', redirectTo: 'login' },
];
