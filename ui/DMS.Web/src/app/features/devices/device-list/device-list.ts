import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';
import { DeviceService } from '../../../core/services/device.service';
import { Device, DeviceStatus, DeviceType } from '../../../core/models/device.model';

@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [
    CommonModule, RouterModule, FormsModule,
    MatCardModule, MatButtonModule, MatIconModule,
    MatInputModule, MatFormFieldModule,
    MatProgressSpinnerModule, MatDividerModule
  ],
  template: `
    <div class="page-container">
      <div class="page-header">
        <h1>Devices</h1>
        <button mat-raised-button color="primary" (click)="openCreate()">
          <mat-icon>add</mat-icon> Add Device
        </button>
      </div>

      <mat-form-field appearance="outline" class="search-field">
        <mat-label>Search devices...</mat-label>
        <input matInput [(ngModel)]="searchTerm" (ngModelChange)="onSearch()"
               placeholder="Dell, MacBook, Samsung..." />
        <mat-icon matSuffix>search</mat-icon>
      </mat-form-field>

      @if (loading()) {
        <div class="spinner-wrapper"><mat-spinner diameter="48" /></div>
      } @else if (devices().length === 0) {
        <p class="empty-state">No devices found.</p>
      } @else {
        <mat-card class="list-card">
          @for (device of devices(); track device.id; let last = $last) {
            <div class="device-row" (click)="goToDevice(device.id)">
              <mat-icon class="device-icon">{{ getTypeIcon(device.type) }}</mat-icon>
              <div class="device-info">
                <div class="device-name">{{ device.name }}</div>
                <div class="device-sub">{{ device.brand }} · {{ device.model }}</div>
              </div>
              <div class="device-meta">
                <span class="status-badge status-{{ device.status }}">
                  {{ getStatusLabel(device.status) }}
                </span>
                @if (device.activeAssignment) {
                  <span class="assigned-to">
                    <mat-icon class="inline-icon">person</mat-icon>
                    {{ device.activeAssignment.userFullName }}
                  </span>
                }
              </div>
              <mat-icon class="chevron">chevron_right</mat-icon>
            </div>
            @if (!last) { <mat-divider /> }
          }
        </mat-card>
      }
    </div>
  `,
  styles: [`
    .page-container { max-width: 900px; margin: 0 auto; padding: 24px 16px; }
    .page-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 24px; }
    .page-header h1 { margin: 0; font-size: 28px; font-weight: 600; }
    .search-field { width: 100%; margin-bottom: 16px; }
    .spinner-wrapper { display: flex; justify-content: center; padding: 64px; }
    .empty-state { text-align: center; color: var(--mat-sys-on-surface-variant); padding: 48px; }
    .list-card { padding: 0; overflow: hidden; }
    .device-row {
      display: flex; align-items: center; gap: 16px;
      padding: 16px 20px; cursor: pointer; transition: background 0.15s;
    }
    .device-row:hover { background: var(--mat-sys-surface-variant); }
    .device-icon { font-size: 28px; width: 28px; height: 28px; color: var(--mat-sys-primary); flex-shrink: 0; }
    .device-info { flex: 1; min-width: 0; }
    .device-name { font-size: 15px; font-weight: 600; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
    .device-sub { font-size: 13px; color: var(--mat-sys-on-surface-variant); }
    .device-meta { display: flex; align-items: center; gap: 12px; flex-shrink: 0; }
    .status-badge { font-size: 12px; font-weight: 500; padding: 2px 10px; border-radius: 12px; white-space: nowrap; }
    .status-1 { background: #e8f5e9; color: #2e7d32; }
    .status-2 { background: #e3f2fd; color: #1565c0; }
    .status-3 { background: #fff8e1; color: #f57f17; }
    .status-4 { background: #fce4ec; color: #c62828; }
    .assigned-to { display: flex; align-items: center; gap: 4px; font-size: 13px; color: var(--mat-sys-on-surface-variant); white-space: nowrap; }
    .inline-icon { font-size: 14px; width: 14px; height: 14px; }
    .chevron { color: var(--mat-sys-on-surface-variant); flex-shrink: 0; }
    @media (max-width: 600px) { .device-meta { display: none; } }
  `]
})
export class DeviceList implements OnInit {
  private deviceService = inject(DeviceService);
  private router = inject(Router);

  devices = signal<Device[]>([]);
  loading = signal(true);
  searchTerm = '';
  private searchTimeout: ReturnType<typeof setTimeout> | null = null;

  ngOnInit(): void {
    this.loadDevices();
  }

  loadDevices(): void {
    this.loading.set(true);
    this.deviceService.getAll({ searchTerm: this.searchTerm || undefined, pageSize: 50 }).subscribe({
      next: (result) => { this.devices.set(result.items); this.loading.set(false); },
      error: () => { this.loading.set(false); this.router.navigate(['/login']); }
    });
  }

  onSearch(): void {
    if (this.searchTimeout) clearTimeout(this.searchTimeout);
    this.searchTimeout = setTimeout(() => this.loadDevices(), 400);
  }

  openCreate(): void { this.router.navigate(['/devices/create']); }
  goToDevice(id: string): void { this.router.navigate(['/devices', id]); }

  getTypeIcon(type: DeviceType): string {
    return ({ 1: 'laptop', 2: 'desktop_windows', 3: 'tablet', 4: 'smartphone', 5: 'monitor', 6: 'cable', 99: 'devices_other' } as Record<number, string>)[type] ?? 'devices_other';
  }

  getStatusLabel(status: DeviceStatus): string {
    return ({ 1: 'Available', 2: 'In Use', 3: 'Maintenance', 4: 'Retired' } as Record<number, string>)[status] ?? 'Unknown';
  }
}
