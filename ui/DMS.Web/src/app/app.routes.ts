import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'devices', pathMatch: 'full' },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login').then(m => m.Login)
  },
  {
    path: 'register',
    loadComponent: () => import('./features/auth/register/register').then(m => m.Register)
  },
  {
    path: 'devices',
    canActivate: [authGuard],
    loadComponent: () => import('./features/devices/device-list/device-list').then(m => m.DeviceList)
  },
  {
    path: 'devices/create',
    canActivate: [authGuard],
    loadComponent: () => import('./features/devices/device-form/device-form').then(m => m.DeviceForm)
  },
  {
    path: 'devices/:id',
    canActivate: [authGuard],
    loadComponent: () => import('./features/devices/device-detail/device-detail').then(m => m.DeviceDetail)
  },
  {
    path: 'devices/:id/edit',
    canActivate: [authGuard],
    loadComponent: () => import('./features/devices/device-form/device-form').then(m => m.DeviceForm)
  },
  {
    path: 'profile',
    canActivate: [authGuard],
    loadComponent: () => import('./features/users/user-profile/user-profile').then(m => m.UserProfile)
  },
  {
    path: 'not-found',
    loadComponent: () => import('./shared/components/not-found/not-found').then(m => m.NotFound)
  },
  { path: '**', redirectTo: 'not-found' }
];
