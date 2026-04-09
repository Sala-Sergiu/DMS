import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    RouterLink,
    RouterLinkActive,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule
  ],
  template: `
    <mat-toolbar color="primary">
      <span class="brand" routerLink="/devices">DMS</span>
      <span class="spacer"></span>
      <a mat-button routerLink="/devices" routerLinkActive="active-link">
        <mat-icon>devices</mat-icon> Devices
      </a>
      <button mat-icon-button [matMenuTriggerFor]="userMenu">
        <mat-icon>account_circle</mat-icon>
      </button>
      <mat-menu #userMenu="matMenu">
        <a mat-menu-item routerLink="/profile">
          <mat-icon>person</mat-icon> Profile
        </a>
        <button mat-menu-item (click)="logout()">
          <mat-icon>logout</mat-icon> Logout
        </button>
      </mat-menu>
    </mat-toolbar>
  `,
  styles: [`
    .brand {
      font-size: 20px;
      font-weight: 600;
      cursor: pointer;
      margin-right: 16px;
    }
    .spacer { flex: 1; }
    .active-link { background: rgba(255,255,255,0.15); border-radius: 4px; }
  `]
})
export class Navbar {
  private authService = inject(AuthService);

  logout(): void {
    this.authService.logout();
  }
}
