import { Component, inject, OnInit, signal } from '@angular/core';
import { SlicePipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatListModule } from '@angular/material/list';
import { RouterModule } from '@angular/router';
import { UserService } from '../../../core/services/user.service';
import { User } from '../../../core/models/user.model';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [
    SlicePipe,
    RouterModule,
    MatCardModule,
    MatIconModule,
    MatDividerModule,
    MatProgressSpinnerModule,
    MatListModule
  ],
  template: `
    @if (loading()) {
      <div class="spinner-container"><mat-spinner /></div>
    } @else if (user()) {
      <div class="profile-page">
        <h1>My Profile</h1>
        <mat-card class="profile-card">
          <mat-card-content>
            <div class="avatar">
              <mat-icon class="avatar-icon">account_circle</mat-icon>
            </div>
            <mat-divider />
            <div class="info-grid">
              <div class="info-item">
                <span class="label">Full Name</span>
                <span class="value">{{ user()!.fullName }}</span>
              </div>
              <div class="info-item">
                <span class="label">Email</span>
                <span class="value">{{ user()!.email }}</span>
              </div>
              <div class="info-item">
                <span class="label">Location</span>
                <span class="value">{{ user()!.location ?? '—' }}</span>
              </div>
              <div class="info-item">
                <span class="label">Member Since</span>
                <span class="value">{{ user()!.createdAtUtc | slice:0:10 }}</span>
              </div>
            </div>

            <mat-divider />

            <h3 class="section-title">Assigned Devices ({{ user()!.activeAssignments.length }})</h3>

            @if (user()!.activeAssignments.length === 0) {
              <p class="empty-state">No devices currently assigned to you.</p>
            } @else {
              <mat-list>
                @for (assignment of user()!.activeAssignments; track assignment.assignmentId) {
                  <mat-list-item>
                    <mat-icon matListItemIcon>devices_other</mat-icon>
                    <span matListItemTitle>{{ assignment.deviceName }}</span>
                    <span matListItemLine>{{ assignment.brand }} {{ assignment.model }}</span>
                    <span matListItemLine class="assigned-date">
                      Assigned {{ assignment.assignedAtUtc | slice:0:10 }}
                    </span>
                  </mat-list-item>
                }
              </mat-list>
            }
          </mat-card-content>
        </mat-card>
      </div>
    }
  `,
  styles: [`
    .spinner-container { display: flex; justify-content: center; padding: 48px; }
    .profile-page { max-width: 640px; margin: 0 auto; }
    h1 { margin-bottom: 24px; }
    .avatar {
      display: flex;
      justify-content: center;
      padding: 24px 0 16px;
    }
    .avatar-icon { font-size: 80px; width: 80px; height: 80px; color: var(--mat-sys-primary); }
    .info-grid {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 20px;
      padding: 20px 0;
    }
    .info-item { display: flex; flex-direction: column; gap: 4px; }
    .label { font-size: 12px; color: var(--mat-sys-on-surface-variant); }
    .value { font-size: 15px; font-weight: 500; }
    .section-title { margin: 20px 0 8px; font-size: 16px; font-weight: 500; }
    .empty-state { color: var(--mat-sys-on-surface-variant); font-size: 14px; }
    .assigned-date { font-size: 12px; color: var(--mat-sys-on-surface-variant); }
  `]
})
export class UserProfile implements OnInit {
  private userService = inject(UserService);

  user = signal<User | null>(null);
  loading = signal(false);

  ngOnInit(): void {
    this.loading.set(true);
    this.userService.getMe().subscribe({
      next: (user) => {
        this.user.set(user);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }
}
