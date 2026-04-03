import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'devices',
    pathMatch: 'full'
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login').then(c => c.Login)
  },
  {
    path: 'register',
    loadComponent: () =>
      import('./features/auth/register/register').then(c => c.Register)
  },
  {
    path: '',
    loadComponent: () =>
      import('./shared/components/layout/layout').then(c => c.Layout),
    canActivate: [authGuard],
    children: [
      {
        path: 'devices',
        loadComponent: () =>
          import('./features/devices/device-list/device-list').then(c => c.DeviceList)
      },
      {
        path: 'devices/new',
        canActivate: [roleGuard(['Manager', 'Admin'])],
        loadComponent: () =>
          import('./features/devices/device-form/device-form').then(c => c.DeviceForm)
      },
      {
        path: 'devices/:id',
        loadComponent: () =>
          import('./features/devices/device-detail/device-detail').then(c => c.DeviceDetail)
      },
      {
        path: 'devices/:id/edit',
        canActivate: [roleGuard(['Manager', 'Admin'])],
        loadComponent: () =>
          import('./features/devices/device-form/device-form').then(c => c.DeviceForm)
      },
      {
        path: 'profile',
        loadComponent: () =>
          import('./features/users/user-profile/user-profile').then(c => c.UserProfile)
      }
    ]
  },
  {
    path: 'forbidden',
    loadComponent: () =>
      import('./shared/components/forbidden/forbidden').then(c => c.Forbidden)
  },
  {
    path: 'not-found',
    loadComponent: () =>
      import('./shared/components/not-found/not-found').then(c => c.NotFound)
  },
  {
    path: '**',
    redirectTo: 'not-found'
  }
];
