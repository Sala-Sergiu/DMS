import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { UserService } from '../../core/services/user.service';
import { DeviceService } from '../../core/services/device.service';
import { User } from '../../core/models/user.model';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule, RouterModule,
    MatCardModule, MatListModule, MatIconModule,
    MatButtonModule, MatChipsModule,
    MatProgressSpinnerModule, MatSnackBarModule
  ],
  template: `
    <div class="profile-container">
      @if (loading()) {
        <div class="spinner-wrapper"><mat-spinner diameter="48" /></div>
      } @else if (user()) {
        <mat-card class="profile-card">
          <mat-card-header>
            <mat-icon mat-card-avatar class="avatar-icon">account_circle</mat-icon>
            <mat-card-title>{{ user()!.fullName }}</mat-card-title>
            <mat-card-subtitle>{{ user()!.email }}</mat-card-subtitle>
          </mat-card-header>

          <mat-card-content>
            <div class="info-section">
              <div class="info-row">
                <mat-icon>location_on</mat-icon>
                <span>{{ user()!.location || 'No location set' }}</span>
              </div>
              <div class="info-row">
                <mat-icon>calendar_today</mat-icon>
                <span>Member since {{ user()!.createdAtUtc | date:'mediumDate' }}</span>
              </div>
            </div>

            <h3 class="section-title">
              <mat-icon>devices</mat-icon>
              Assigned Devices
              <mat-chip class="count-chip">{{ user()!.activeAssignments.length }}</mat-chip>
            </h3>

            @if (user()!.activeAssignments.length === 0) {
              <p class="empty-state">No devices currently assigned to you.</p>
            } @else {
              <mat-list>
                @for (assignment of user()!.activeAssignments; track assignment.assignmentId) {
                  <mat-list-item class="device-item">
                    <mat-icon matListItemIcon>devices_other</mat-icon>
                    <span matListItemTitle>{{ assignment.deviceName }}</span>
                    <span matListItemLine>{{ assignment.brand }} {{ assignment.model }}</span>
                    <span matListItemLine class="assigned-date">
                      Assigned {{ assignment.assignedAtUtc | date:'mediumDate' }}
                    </span>
                    <div matListItemMeta class="item-actions">
                      <button mat-icon-button (click)="goToDevice(assignment.deviceId)" title="View device">
                        <mat-icon>open_in_new</mat-icon>
                      </button>
                      <button mat-icon-button color="warn"
                              [disabled]="unassigningId() === assignment.deviceId"
                              (click)="unassign(assignment.deviceId, $event)"
                              title="Unassign device">
                        @if (unassigningId() === assignment.deviceId) {
                          <mat-spinner diameter="18" />
                        } @else {
                          <mat-icon>link_off</mat-icon>
                        }
                      </button>
                    </div>
                  </mat-list-item>
                }
              </mat-list>
            }
          </mat-card-content>
        </mat-card>
      }
    </div>
  `,
  styles: [`
    .profile-container { max-width: 680px; margin: 32px auto; padding: 0 16px; }
    .spinner-wrapper { display: flex; justify-content: center; padding: 48px; }
    .profile-card { padding: 8px; }
    .avatar-icon { font-size: 40px; width: 40px; height: 40px; color: var(--mat-sys-primary); }
    .info-section { display: flex; flex-direction: column; gap: 8px; margin: 16px 0; }
    .info-row {
      display: flex; align-items: center; gap: 8px;
      color: var(--mat-sys-on-surface-variant); font-size: 14px;
    }
    .section-title {
      display: flex; align-items: center; gap: 8px;
      font-size: 16px; font-weight: 500; margin: 24px 0 8px;
    }
    .count-chip { font-size: 12px; min-height: 22px; }
    .empty-state { color: var(--mat-sys-on-surface-variant); font-size: 14px; padding: 8px 0; }
    .device-item { border-bottom: 1px solid var(--mat-sys-outline-variant); }
    .assigned-date { font-size: 12px; color: var(--mat-sys-on-surface-variant); }
    .item-actions { display: flex; align-items: center; gap: 4px; }
  `]
})
export class Profile implements OnInit {
  private userService = inject(UserService);
  private deviceService = inject(DeviceService);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  user = signal<User | null>(null);
  loading = signal(true);
  unassigningId = signal<string | null>(null);

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.loading.set(true);
    this.userService.getMe().subscribe({
      next: (u) => { this.user.set(u); this.loading.set(false); },
      error: () => { this.loading.set(false); }
    });
  }

  unassign(deviceId: string, event: Event): void {
    event.stopPropagation();
    this.unassigningId.set(deviceId);
    this.deviceService.unassignFromSelf(deviceId).subscribe({
      next: () => {
        this.snackBar.open('Device unassigned.', 'Close', { duration: 3000 });
        this.unassigningId.set(null);
        this.loadProfile(); // reîncarcă profilul pentru a actualiza lista
      },
      error: (err) => {
        this.snackBar.open(err.error?.message ?? 'Unassign failed.', 'Close', { duration: 4000 });
        this.unassigningId.set(null);
      }
    });
  }

  goToDevice(deviceId: string): void {
    this.router.navigate(['/devices', deviceId]);
  }
}
