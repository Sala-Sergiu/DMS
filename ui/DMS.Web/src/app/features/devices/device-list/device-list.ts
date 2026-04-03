import { Component, inject, OnInit, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { DeviceService } from '../../../core/services/device.service';
import { AuthService } from '../../../core/services/auth.service';
import { Device } from '../../../core/models/device.model';
import { DeviceType } from '../../../core/models/device-type.enum';
import { DeviceStatus } from '../../../core/models/device-status.enum';

@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [
    RouterLink,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    MatTooltipModule
  ],
  template: `
    <div class="page-header">
      <h1>Devices</h1>
      @if (isManagerOrAdmin()) {
        <a mat-raised-button color="primary" routerLink="/devices/new">
          <mat-icon>add</mat-icon> Add Device
        </a>
      }
    </div>

    <div class="filters">
      <mat-form-field appearance="outline" class="search-field">
        <mat-label>Search</mat-label>
        <input matInput [formControl]="searchControl" placeholder="Name, brand, model..." />
        <mat-icon matSuffix>search</mat-icon>
      </mat-form-field>

      <mat-form-field appearance="outline">
        <mat-label>Type</mat-label>
        <mat-select [formControl]="typeControl">
          <mat-option [value]="null">All Types</mat-option>
          @for (type of deviceTypes; track type.value) {
            <mat-option [value]="type.value">{{ type.label }}</mat-option>
          }
        </mat-select>
      </mat-form-field>

      <mat-form-field appearance="outline">
        <mat-label>Status</mat-label>
        <mat-select [formControl]="statusControl">
          <mat-option [value]="null">All Statuses</mat-option>
          @for (status of deviceStatuses; track status.value) {
            <mat-option [value]="status.value">{{ status.label }}</mat-option>
          }
        </mat-select>
      </mat-form-field>
    </div>

    @if (loading()) {
      <div class="spinner-container">
        <mat-spinner />
      </div>
    } @else {
      <div class="table-container mat-elevation-z2">
        <table mat-table [dataSource]="devices()" matSort (matSortChange)="onSort($event)">

          <ng-container matColumnDef="name">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Name</th>
            <td mat-cell *matCellDef="let d">{{ d.name }}</td>
          </ng-container>

          <ng-container matColumnDef="brand">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Brand</th>
            <td mat-cell *matCellDef="let d">{{ d.brand }}</td>
          </ng-container>

          <ng-container matColumnDef="model">
            <th mat-header-cell *matHeaderCellDef>Model</th>
            <td mat-cell *matCellDef="let d">{{ d.model }}</td>
          </ng-container>

          <ng-container matColumnDef="type">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Type</th>
            <td mat-cell *matCellDef="let d">{{ getTypeLabel(d.type) }}</td>
          </ng-container>

          <ng-container matColumnDef="status">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Status</th>
            <td mat-cell *matCellDef="let d">
              <span [class]="'status-badge status-' + d.status">
                {{ getStatusLabel(d.status) }}
              </span>
            </td>
          </ng-container>

          <ng-container matColumnDef="assignedTo">
            <th mat-header-cell *matHeaderCellDef>Assigned To</th>
            <td mat-cell *matCellDef="let d">
              {{ d.activeAssignment?.userFullName ?? '—' }}
            </td>
          </ng-container>

          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef></th>
            <td mat-cell *matCellDef="let d">
              <a mat-icon-button [routerLink]="['/devices', d.id]" matTooltip="View details">
                <mat-icon>visibility</mat-icon>
              </a>
              @if (isManagerOrAdmin()) {
                <a mat-icon-button [routerLink]="['/devices', d.id, 'edit']" matTooltip="Edit">
                  <mat-icon>edit</mat-icon>
                </a>
              }
            </td>
          </ng-container>

          <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns;"
              class="table-row" [routerLink]="['/devices', row.id]"></tr>
        </table>

        <mat-paginator
          [length]="totalCount()"
          [pageSize]="pageSize"
          [pageSizeOptions]="[10, 20, 50]"
          (page)="onPage($event)"
          showFirstLastButtons />
      </div>
    }
  `,
  styles: [`
    .page-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 24px;
      h1 { margin: 0; }
    }
    .filters {
      display: flex;
      gap: 16px;
      margin-bottom: 16px;
      flex-wrap: wrap;
    }
    .search-field { flex: 1; min-width: 200px; }
    .table-container { overflow-x: auto; }
    table { width: 100%; }
    .table-row { cursor: pointer; }
    .table-row:hover { background: var(--mat-sys-surface-variant); }
    .spinner-container {
      display: flex;
      justify-content: center;
      padding: 48px;
    }
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
export class DeviceList implements OnInit {
  private deviceService = inject(DeviceService);
  private authService = inject(AuthService);
  private router = inject(Router);

  devices = signal<Device[]>([]);
  totalCount = signal(0);
  loading = signal(false);

  displayedColumns = ['name', 'brand', 'model', 'type', 'status', 'assignedTo', 'actions'];

  searchControl = new FormControl('');
  typeControl = new FormControl<DeviceType | null>(null);
  statusControl = new FormControl<DeviceStatus | null>(null);

  pageNumber = 1;
  pageSize = 20;
  sortBy = 'name';
  sortDescending = false;

  deviceTypes = [
    { value: DeviceType.Laptop, label: 'Laptop' },
    { value: DeviceType.Desktop, label: 'Desktop' },
    { value: DeviceType.Tablet, label: 'Tablet' },
    { value: DeviceType.Smartphone, label: 'Smartphone' },
    { value: DeviceType.Monitor, label: 'Monitor' },
    { value: DeviceType.Peripheral, label: 'Peripheral' },
    { value: DeviceType.Other, label: 'Other' }
  ];

  deviceStatuses = [
    { value: DeviceStatus.Available, label: 'Available' },
    { value: DeviceStatus.InUse, label: 'In Use' },
    { value: DeviceStatus.UnderMaintenance, label: 'Under Maintenance' },
    { value: DeviceStatus.Retired, label: 'Retired' }
  ];

  ngOnInit(): void {
    this.loadDevices();

    this.searchControl.valueChanges.pipe(
      debounceTime(400),
      distinctUntilChanged()
    ).subscribe(() => {
      this.pageNumber = 1;
      this.loadDevices();
    });

    this.typeControl.valueChanges.subscribe(() => {
      this.pageNumber = 1;
      this.loadDevices();
    });

    this.statusControl.valueChanges.subscribe(() => {
      this.pageNumber = 1;
      this.loadDevices();
    });
  }

  loadDevices(): void {
    this.loading.set(true);
    this.deviceService.getPaged({
      searchTerm: this.searchControl.value ?? undefined,
      type: this.typeControl.value ?? undefined,
      status: this.statusControl.value ?? undefined,
      sortBy: this.sortBy,
      sortDescending: this.sortDescending,
      pageNumber: this.pageNumber,
      pageSize: this.pageSize
    }).subscribe({
      next: (response) => {
        this.devices.set(response.body?.items ?? []);
        const total = response.headers.get('X-Total-Count');
        this.totalCount.set(total ? parseInt(total) : (response.body?.totalCount ?? 0));
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  onSort(sort: Sort): void {
    this.sortBy = sort.active;
    this.sortDescending = sort.direction === 'desc';
    this.loadDevices();
  }

  onPage(event: PageEvent): void {
    this.pageNumber = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadDevices();
  }

  isManagerOrAdmin(): boolean {
    const role = this.authService.getRole();
    return role === 'Manager' || role === 'Admin';
  }

  getTypeLabel(type: DeviceType): string {
    return this.deviceTypes.find(t => t.value === type)?.label ?? '—';
  }

  getStatusLabel(status: DeviceStatus): string {
    return this.deviceStatuses.find(s => s.value === status)?.label ?? '—';
  }
}
