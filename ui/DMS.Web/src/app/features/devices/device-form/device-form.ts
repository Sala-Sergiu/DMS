import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { DeviceService } from '../../../core/services/device.service';
import { DeviceType } from '../../../core/models/device-type.enum';

@Component({
  selector: 'app-device-form',
  standalone: true,
  imports: [
    RouterLink,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatDatepickerModule,
    MatNativeDateModule
  ],
  template: `
    <div class="form-page">
      <div class="page-header">
        <button mat-icon-button routerLink="/devices">
          <mat-icon>arrow_back</mat-icon>
        </button>
        <h1>{{ isEditMode() ? 'Edit Device' : 'Add Device' }}</h1>
      </div>

      <mat-card class="form-card">
        <mat-card-content>
          <form [formGroup]="form" (ngSubmit)="onSubmit()">
            <div class="form-grid">
              <mat-form-field appearance="outline">
                <mat-label>Name</mat-label>
                <input matInput formControlName="name" />
                @if (form.get('name')?.hasError('required') && form.get('name')?.touched) {
                  <mat-error>Name is required</mat-error>
                }
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Brand</mat-label>
                <input matInput formControlName="brand" />
                @if (form.get('brand')?.hasError('required') && form.get('brand')?.touched) {
                  <mat-error>Brand is required</mat-error>
                }
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Model</mat-label>
                <input matInput formControlName="model" />
                @if (form.get('model')?.hasError('required') && form.get('model')?.touched) {
                  <mat-error>Model is required</mat-error>
                }
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Type</mat-label>
                <mat-select formControlName="type">
                  @for (t of deviceTypes; track t.value) {
                    <mat-option [value]="t.value">{{ t.label }}</mat-option>
                  }
                </mat-select>
              </mat-form-field>

              @if (!isEditMode()) {
                <mat-form-field appearance="outline">
                  <mat-label>Serial Number</mat-label>
                  <input matInput formControlName="serialNumber" />
                  @if (form.get('serialNumber')?.hasError('required') && form.get('serialNumber')?.touched) {
                    <mat-error>Serial number is required</mat-error>
                  }
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Asset Tag</mat-label>
                  <input matInput formControlName="assetTag" />
                  @if (form.get('assetTag')?.hasError('required') && form.get('assetTag')?.touched) {
                    <mat-error>Asset tag is required</mat-error>
                  }
                </mat-form-field>

                <mat-form-field appearance="outline">
                  <mat-label>Purchase Date</mat-label>
                  <input matInput [matDatepicker]="picker" formControlName="purchasedAtUtc" />
                  <mat-datepicker-toggle matIconSuffix [for]="picker" />
                  <mat-datepicker #picker />
                </mat-form-field>
              }
            </div>

            @if (errorMessage()) {
              <p class="error-message">{{ errorMessage() }}</p>
            }

            <div class="form-actions">
              <a mat-stroked-button routerLink="/devices">Cancel</a>
              <button mat-raised-button color="primary" type="submit" [disabled]="loading()">
                @if (loading()) {
                  <mat-spinner diameter="20" />
                } @else {
                  {{ isEditMode() ? 'Save Changes' : 'Create Device' }}
                }
              </button>
            </div>
          </form>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .form-page { max-width: 800px; margin: 0 auto; }
    .page-header {
      display: flex;
      align-items: center;
      gap: 8px;
      margin-bottom: 24px;
      h1 { margin: 0; }
    }
    .form-grid {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 16px;
    }
    .form-actions {
      display: flex;
      justify-content: flex-end;
      gap: 12px;
      margin-top: 24px;
    }
    .error-message { color: var(--mat-sys-error); font-size: 14px; }
  `]
})
export class DeviceForm implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private deviceService = inject(DeviceService);
  private fb = inject(FormBuilder);

  isEditMode = signal(false);
  loading = signal(false);
  errorMessage = signal('');
  private deviceId: string | null = null;

  deviceTypes = [
    { value: DeviceType.Laptop, label: 'Laptop' },
    { value: DeviceType.Desktop, label: 'Desktop' },
    { value: DeviceType.Tablet, label: 'Tablet' },
    { value: DeviceType.Smartphone, label: 'Smartphone' },
    { value: DeviceType.Monitor, label: 'Monitor' },
    { value: DeviceType.Peripheral, label: 'Peripheral' },
    { value: DeviceType.Other, label: 'Other' }
  ];

  form: FormGroup = this.fb.group({
    name: ['', Validators.required],
    brand: ['', Validators.required],
    model: ['', Validators.required],
    type: [DeviceType.Laptop, Validators.required],
    serialNumber: [''],
    assetTag: [''],
    purchasedAtUtc: [null]
  });

  ngOnInit(): void {
    this.deviceId = this.route.snapshot.paramMap.get('id');
    this.isEditMode.set(!!this.deviceId && this.route.snapshot.url.some(s => s.path === 'edit'));

    if (!this.isEditMode()) {
      this.form.get('serialNumber')?.addValidators(Validators.required);
      this.form.get('assetTag')?.addValidators(Validators.required);
      this.form.get('serialNumber')?.updateValueAndValidity();
      this.form.get('assetTag')?.updateValueAndValidity();
    }

    if (this.isEditMode() && this.deviceId) {
      this.deviceService.getById(this.deviceId).subscribe(device => {
        this.form.patchValue({
          name: device.name,
          brand: device.brand,
          model: device.model,
          type: device.type
        });
      });
    }
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.errorMessage.set('');

    if (this.isEditMode() && this.deviceId) {
      const { name, brand, model, type } = this.form.getRawValue();
      this.deviceService.update(this.deviceId, { name, brand, model, type }).subscribe({
        next: () => this.router.navigate(['/devices', this.deviceId]),
        error: (err) => {
          this.errorMessage.set(err.error?.message ?? 'Update failed.');
          this.loading.set(false);
        }
      });
    } else {
      const value = this.form.getRawValue();
      this.deviceService.create({
        ...value,
        purchasedAtUtc: value.purchasedAtUtc
          ? new Date(value.purchasedAtUtc).toISOString()
          : null
      }).subscribe({
        next: (device) => this.router.navigate(['/devices', device.id]),
        error: (err) => {
          this.errorMessage.set(err.error?.message ?? 'Create failed.');
          this.loading.set(false);
        }
      });
    }
  }
}
