import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { SlicePipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialogModule } from '@angular/material/dialog';
import { DeviceService } from '../../../core/services/device.service';
import { AuthService } from '../../../core/services/auth.service';
import { Device } from '../../../core/models/device.model';
import { DeviceType } from '../../../core/models/device-type.enum';
import { DeviceStatus } from '../../../core/models/device-status.enum';

@Component({
  selector: 'app-device-detail',
  standalone: true,
  imports: [
    RouterLink,
    SlicePipe,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatDialogModule
  ],
  template: `
    @if (loading()) {
      <div class="spinner-container"><mat-spinner /></div>
    } @else if (device()) {
      <div class="detail-page">
        <div class="page-header">
          <button mat-icon-button routerLink="/devices">
            <mat-icon>arrow_back</mat-icon>
          </button>
          <h1>{{ device()!.name }}</h1>
          <span [class]="'status-badge status-' + device()!.status">
            {{ getStatusLabel(device()!.status) }}
          </span>
          <span class="spacer"></span>
          @if (isManagerOrAdmin()) {
            <a mat-stroked-button [routerLink]="['/devices', device()!.id, 'edit']">
              <mat-icon>edit</mat-icon> Edit
            </a>
          }
        </div>

        <div class="detail-grid">
          <mat-card>
            <mat-card-header>
              <mat-card-title>Device Information</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              <div class="info-grid">
                <div class="info-item">
                  <span class="label">Brand</span>
                  <span class="value">{{ device()!.brand }}</span>
                </div>
                <div class="info-item">
                  <span class="label">Model</span>
                  <span class="value">{{ device()!.model }}</span>
                </div>
                <div class="info-item">
                  <span class="label">Type</span>
                  <span class="value">{{ getTypeLabel(device()!.type) }}</span>
                </div>
                <div class="info-item">
                  <span class="label">Serial Number</span>
                  <span class="value">{{ device()!.serialNumber }}</span>
                </div>
                <div class="info-item">
                  <span class="label">Asset Tag</span>
                  <span class="value">{{ device()!.assetTag }}</span>
                </div>
                <div class="info-item">
                  <span class="label">Purchased</span>
                  <span class="value">
                    {{ device()!.purchasedAtUtc ? (device()!.purchasedAtUtc | slice:0:10) : '—' }}
                  </span>
                </div>
              </div>
            </mat-card-content>
          </mat-card>

          <mat-card>
            <mat-card-header>
              <mat-card-title>Assignment</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              @if (device()!.activeAssignment) {
                <div class="info-grid">
                  <div class="info-item">
                    <span class="label">Assigned To</span>
                    <span class="value">{{ device()!.activeAssignment!.userFullName }}</span>
                  </div>
                  <div class="info-item">
                    <span class="label">Since</span>
                    <span class="value">
                      {{ device()!.activeAssignment!.assignedAtUtc | slice:0:10 }}
                    </span>
                  </div>
                </div>
                @if (isManagerOrAdmin()) {
                  <button mat-stroked-button color="warn" (click)="returnDevice()" class="action-btn">
                    <mat-icon>assignment_return</mat-icon> Return Device
                  </button>
                }
              } @else {
                <p class="no-assignment">Not currently assigned.</p>
                @if (isManagerOrAdmin()) {
                  <button mat-raised-button color="primary" (click)="assignDevice()" class="action-btn">
                    <mat-icon>assignment_ind</mat-icon> Assign Device
                  </button>
                }
              }
            </mat-card-content>
          </mat-card>
        </div>
      </div>
    }
  `,
  styles: [`
    .spinner-container { display: flex; justify-content: center; padding: 48px; }
    .page-header {
      display: flex;
      align-items: center;
      gap: 12px;
      margin-bottom: 24px;
      h1 { margin: 0; }
    }
    .spacer { flex: 1; }
    .detail-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(320px, 1fr));
      gap: 24px;
    }
    .info-grid {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 16px;
      padding-top: 8px;
    }
    .info-item { display: flex; flex-direction: column; gap: 4px; }
    .label { font-size: 12px; color: var(--mat-sys-on-surface-variant); }
    .value { font-size: 15px; font-weight: 500; }
    .no-assignment { color: var(--mat-sys-on-surface-variant); }
    .action-btn { margin-top: 16px; }
    .status-badge {
      padding: 4px 10px;
      border-radius: 12px;
      font-size: 12px;
      font-weight: 500;
    }
    .status-badge.status-1 { background: #e8f5e9; color: #2e7d32; }
    .status-badge.status-2 { background: #e3f2fd; color: #1565c0; }
    .status-badge.status-3 { background: #fff8e1; color: #f57f17; }
    .status-badge.status-4 { background: #fce4ec; color: #c62828; }
  `]
})
export class DeviceDetail implements OnInit {
  private route = inject(ActivatedRoute);
  private deviceService = inject(DeviceService);
  private authService = inject(AuthService);
  private router = inject(Router);

  device = signal<Device | null>(null);
  loading = signal(false);

  readonly typeLabels: Record<DeviceType, string> = {
    [DeviceType.Laptop]: 'Laptop',
    [DeviceType.Desktop]: 'Desktop',
    [DeviceType.Tablet]: 'Tablet',
    [DeviceType.Smartphone]: 'Smartphone',
    [DeviceType.Monitor]: 'Monitor',
    [DeviceType.Peripheral]: 'Peripheral',
    [DeviceType.Other]: 'Other'
  };

  readonly statusLabels: Record<DeviceStatus, string> = {
    [DeviceStatus.Available]: 'Available',
    [DeviceStatus.InUse]: 'In Use',
    [DeviceStatus.UnderMaintenance]: 'Under Maintenance',
    [DeviceStatus.Retired]: 'Retired'
  };

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id')!;
    this.loading.set(true);
    this.deviceService.getById(id).subscribe({
      next: (device) => {
        this.device.set(device);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  returnDevice(): void {
    this.deviceService.return(this.device()!.id, {}).subscribe({
      next: (updated) => this.device.set(updated)
    });
  }

  assignDevice(): void {
    const userId = prompt('Enter User ID to assign:');
    if (!userId) return;
    this.deviceService.assign(this.device()!.id, { userId }).subscribe({
      next: (updated) => this.device.set(updated)
    });
  }

  isManagerOrAdmin(): boolean {
    const role = this.authService.getRole();
    return role === 'Manager' || role === 'Admin';
  }

  getTypeLabel(type: DeviceType): string {
    return this.typeLabels[type] ?? '—';
  }

  getStatusLabel(status: DeviceStatus): string {
    return this.statusLabels[status] ?? '—';
  }
}
