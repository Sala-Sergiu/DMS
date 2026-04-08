import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { DeviceService } from '../../../core/services/device.service';
import { AuthService } from '../../../core/services/auth.service';
import { Device, DeviceStatus, DeviceType } from '../../../core/models/device.model';

@Component({
  selector: 'app-device-detail',
  standalone: true,
  imports: [
    CommonModule, RouterModule,
    MatCardModule, MatButtonModule, MatIconModule,
    MatDividerModule, MatProgressSpinnerModule, MatSnackBarModule
  ],
  template: `
    <div class="page-container">
      @if (loading()) {
        <div class="spinner-wrapper"><mat-spinner diameter="48" /></div>
      } @else if (device()) {
        <div class="page-header">
          <button mat-icon-button (click)="goBack()"><mat-icon>arrow_back</mat-icon></button>
          <h1>{{ device()!.name }}</h1>
          <div class="header-actions">
            <button mat-stroked-button (click)="editDevice()">
              <mat-icon>edit</mat-icon> Edit
            </button>
            <button mat-stroked-button color="warn" (click)="deleteDevice()">
              <mat-icon>delete</mat-icon> Delete
            </button>
          </div>
        </div>

        <div class="content-grid">
          <!-- Device Info -->
          <mat-card>
            <mat-card-header>
              <mat-icon mat-card-avatar class="device-icon">{{ getTypeIcon(device()!.type) }}</mat-icon>
              <mat-card-title>Device Details</mat-card-title>
              <mat-card-subtitle>
                <span class="status-badge status-{{ device()!.status }}">
                  {{ getStatusLabel(device()!.status) }}
                </span>
              </mat-card-subtitle>
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
                @if (device()!.operatingSystem) {
                  <div class="info-item">
                    <span class="label">Operating System</span>
                    <span class="value">
                      {{ device()!.operatingSystem }}
                      @if (device()!.osVersion) { {{ device()!.osVersion }} }
                    </span>
                  </div>
                }
                @if (device()!.processor) {
                  <div class="info-item">
                    <span class="label">Processor</span>
                    <span class="value">{{ device()!.processor }}</span>
                  </div>
                }
                @if (device()!.ramGb) {
                  <div class="info-item">
                    <span class="label">RAM</span>
                    <span class="value">{{ device()!.ramGb }} GB</span>
                  </div>
                }
                @if (device()!.purchasedAtUtc) {
                  <div class="info-item">
                    <span class="label">Purchased</span>
                    <span class="value">{{ device()!.purchasedAtUtc | date:'mediumDate' }}</span>
                  </div>
                }
              </div>

              @if (device()!.description) {
                <div class="description-section">
                  <span class="label">Description</span>
                  <p class="description-text">{{ device()!.description }}</p>
                </div>
              }
            </mat-card-content>
          </mat-card>

          <!-- Assignment + AI -->
          <mat-card>
            <mat-card-header>
              <mat-card-title>Assignment</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              @if (device()!.activeAssignment; as assignment) {
                <div class="assignment-info">
                  <mat-icon class="assignment-icon">person</mat-icon>
                  <div>
                    <div class="assigned-name">{{ assignment.userFullName }}</div>
                    <div class="assigned-date">Since {{ assignment.assignedAtUtc | date:'mediumDate' }}</div>
                  </div>
                </div>
                @if (isAssignedToMe()) {
                  <button mat-stroked-button color="warn" class="action-btn"
                          (click)="unassign()" [disabled]="actionLoading()">
                    <mat-icon>link_off</mat-icon> Unassign from me
                  </button>
                }
              } @else if (device()!.status === DeviceStatus.Available) {
                <p class="no-assignment">This device is currently unassigned.</p>
                <button mat-raised-button color="primary" class="action-btn"
                        (click)="assignToMe()" [disabled]="actionLoading()">
                  <mat-icon>link</mat-icon> Assign to me
                </button>
              } @else {
                <p class="no-assignment">Device is not available for assignment.</p>
              }
            </mat-card-content>

            <mat-divider style="margin: 16px 0;" />

            <mat-card-header>
              <mat-card-title>AI Description</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              @if (generatedDescription()) {
                <p class="description-text">{{ generatedDescription() }}</p>
              }
              <button mat-stroked-button class="action-btn"
                      (click)="generateDescription()" [disabled]="generatingDescription()">
                @if (generatingDescription()) {
                  <mat-spinner diameter="18" />
                } @else {
                  <mat-icon>auto_awesome</mat-icon>
                }
                {{ generatedDescription() ? 'Regenerate' : 'Generate' }} Description
              </button>
              @if (descriptionError()) {
                <p class="error-text">{{ descriptionError() }}</p>
              }
            </mat-card-content>
          </mat-card>
        </div>
      } @else {
        <div class="spinner-wrapper"><p>Device not found.</p></div>
      }
    </div>
  `,
  styles: [`
    .page-container { max-width: 1000px; margin: 0 auto; padding: 24px 16px; }
    .spinner-wrapper { display: flex; justify-content: center; padding: 64px; }
    .page-header { display: flex; align-items: center; gap: 12px; margin-bottom: 24px; }
    .page-header h1 { margin: 0; flex: 1; font-size: 26px; }
    .header-actions { display: flex; gap: 8px; }
    .content-grid { display: grid; grid-template-columns: 1fr 340px; gap: 16px; }
    .device-icon { font-size: 32px; width: 32px; height: 32px; color: var(--mat-sys-primary); }
    .status-badge { font-size: 12px; font-weight: 500; padding: 2px 10px; border-radius: 12px; display: inline-block; }
    .status-1 { background: #e8f5e9; color: #2e7d32; }
    .status-2 { background: #e3f2fd; color: #1565c0; }
    .status-3 { background: #fff8e1; color: #f57f17; }
    .status-4 { background: #fce4ec; color: #c62828; }
    .info-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; padding-top: 12px; }
    .info-item { display: flex; flex-direction: column; gap: 2px; }
    .label { font-size: 12px; color: var(--mat-sys-on-surface-variant); }
    .value { font-size: 15px; font-weight: 500; }
    .description-section { margin-top: 20px; display: flex; flex-direction: column; gap: 6px; }
    .description-text { font-size: 14px; line-height: 1.6; background: var(--mat-sys-surface-variant); padding: 12px; border-radius: 8px; margin: 0; }
    .assignment-info { display: flex; align-items: center; gap: 12px; margin-bottom: 16px; }
    .assignment-icon { font-size: 36px; width: 36px; height: 36px; color: var(--mat-sys-primary); }
    .assigned-name { font-weight: 600; font-size: 15px; }
    .assigned-date { font-size: 12px; color: var(--mat-sys-on-surface-variant); }
    .action-btn { width: 100%; margin-top: 8px; }
    .no-assignment { color: var(--mat-sys-on-surface-variant); font-size: 14px; margin-bottom: 8px; }
    .error-text { color: var(--mat-sys-error); font-size: 13px; margin-top: 8px; }
    @media (max-width: 768px) { .content-grid { grid-template-columns: 1fr; } }
  `]
})
export class DeviceDetail implements OnInit {
  private deviceService = inject(DeviceService);
  private authService = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private snackBar = inject(MatSnackBar);

  readonly DeviceStatus = DeviceStatus;

  device = signal<Device | null>(null);
  loading = signal(true);
  actionLoading = signal(false);
  generatingDescription = signal(false);
  generatedDescription = signal('');
  descriptionError = signal('');

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id')!;
    this.loadDevice(id);
  }

  loadDevice(id: string): void {
    this.loading.set(true);
    this.deviceService.getById(id).subscribe({
      next: (d) => { this.device.set(d); this.loading.set(false); },
      error: () => { this.loading.set(false); }
    });
  }

  isAssignedToMe(): boolean {
    const assignment = this.device()?.activeAssignment;
    if (!assignment) return false;
    const token = this.authService.getToken();
    if (!token) return false;
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return assignment.userId === payload['sub'];
    } catch { return false; }
  }

  assignToMe(): void {
    const d = this.device();
    if (!d) return;
    this.actionLoading.set(true);
    this.deviceService.assignToSelf(d.id).subscribe({
      next: (updated) => { this.device.set(updated); this.actionLoading.set(false); this.snackBar.open('Device assigned.', 'Close', { duration: 3000 }); },
      error: (err) => { this.snackBar.open(err.error?.message ?? 'Assignment failed.', 'Close', { duration: 4000 }); this.actionLoading.set(false); }
    });
  }

  unassign(): void {
    const d = this.device();
    if (!d) return;
    this.actionLoading.set(true);
    this.deviceService.unassignFromSelf(d.id).subscribe({
      next: (updated) => { this.device.set(updated); this.actionLoading.set(false); this.snackBar.open('Device unassigned.', 'Close', { duration: 3000 }); },
      error: (err) => { this.snackBar.open(err.error?.message ?? 'Unassign failed.', 'Close', { duration: 4000 }); this.actionLoading.set(false); }
    });
  }

  generateDescription(): void {
    const d = this.device();
    if (!d) return;
    this.generatingDescription.set(true);
    this.descriptionError.set('');
    this.deviceService.generateDescription({ name: d.name, brand: d.brand, model: d.model, type: d.type }).subscribe({
      next: (res) => { this.generatedDescription.set(res.description); this.generatingDescription.set(false); },
      error: (err) => { this.descriptionError.set(err.error?.message ?? 'Generation failed.'); this.generatingDescription.set(false); }
    });
  }

  editDevice(): void { this.router.navigate(['/devices', this.device()!.id, 'edit']); }

  deleteDevice(): void {
    if (!confirm(`Delete "${this.device()!.name}"?`)) return;
    this.deviceService.delete(this.device()!.id).subscribe({
      next: () => { this.snackBar.open('Device deleted.', 'Close', { duration: 3000 }); this.router.navigate(['/devices']); },
      error: (err) => { this.snackBar.open(err.error?.message ?? 'Delete failed.', 'Close', { duration: 4000 }); }
    });
  }

  goBack(): void { this.router.navigate(['/devices']); }

  // 1=Smartphone, 2=Tablet, 3=Laptop, 99=Other
  getTypeIcon(type: DeviceType): string {
    return ({
      [DeviceType.Smartphone]: 'smartphone',
      [DeviceType.Tablet]: 'tablet',
      [DeviceType.Laptop]: 'laptop',
      [DeviceType.Other]: 'devices_other'
    } as Record<number, string>)[type] ?? 'devices_other';
  }

  getTypeLabel(type: DeviceType): string {
    return ({
      [DeviceType.Smartphone]: 'Smartphone',
      [DeviceType.Tablet]: 'Tablet',
      [DeviceType.Laptop]: 'Laptop',
      [DeviceType.Other]: 'Other'
    } as Record<number, string>)[type] ?? 'Unknown';
  }

  getStatusLabel(status: DeviceStatus): string {
    return ({
      [DeviceStatus.Available]: 'Available',
      [DeviceStatus.InUse]: 'In Use',
      [DeviceStatus.UnderMaintenance]: 'Maintenance',
      [DeviceStatus.Retired]: 'Retired'
    } as Record<number, string>)[status] ?? 'Unknown';
  }
}
