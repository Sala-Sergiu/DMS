import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { DeviceService } from '../../../core/services/device.service';
import { DeviceType } from '../../../core/models/device.model';

@Component({
  selector: 'app-device-form',
  standalone: true,
  imports: [
    CommonModule, RouterModule, ReactiveFormsModule,
    MatCardModule, MatButtonModule, MatIconModule,
    MatInputModule, MatFormFieldModule, MatSelectModule, MatProgressSpinnerModule
  ],
  template: `
    <div class="page-container">
      <div class="page-header">
        <button mat-icon-button (click)="goBack()"><mat-icon>arrow_back</mat-icon></button>
        <h1>{{ isEdit ? 'Edit Device' : 'Add Device' }}</h1>
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
                <mat-label>Brand (Manufacturer)</mat-label>
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

              <mat-form-field appearance="outline">
                <mat-label>Operating System</mat-label>
                <input matInput formControlName="operatingSystem" placeholder="iOS, Android, Windows..." />
                <mat-hint>Optional</mat-hint>
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>OS Version</mat-label>
                <input matInput formControlName="osVersion" placeholder="17.0, 14, 11..." />
                <mat-hint>Optional</mat-hint>
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Processor</mat-label>
                <input matInput formControlName="processor" placeholder="A17 Pro, Snapdragon 8 Gen 3..." />
                <mat-hint>Optional</mat-hint>
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>RAM (GB)</mat-label>
                <input matInput type="number" formControlName="ramGb" min="1" />
                <mat-hint>Optional</mat-hint>
              </mat-form-field>

              @if (!isEdit) {
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
                  <input matInput type="date" formControlName="purchasedAtUtc" />
                </mat-form-field>
              }

              <mat-form-field appearance="outline" class="full-width">
                <mat-label>Description</mat-label>
                <textarea matInput formControlName="description" rows="3"
                          placeholder="Optional manual description..."></textarea>
                <mat-hint>Optional — or use AI Generate on the detail page</mat-hint>
              </mat-form-field>

            </div>

            @if (errorMessage) {
              <p class="error-message">{{ errorMessage }}</p>
            }

            <div class="form-actions">
              <button mat-button type="button" (click)="goBack()">Cancel</button>
              <button mat-raised-button color="primary" type="submit" [disabled]="loading">
                @if (loading) { <mat-spinner diameter="20" /> }
                @else { {{ isEdit ? 'Save Changes' : 'Create Device' }} }
              </button>
            </div>
          </form>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .page-container { max-width: 720px; margin: 0 auto; padding: 24px 16px; }
    .page-header { display: flex; align-items: center; gap: 8px; margin-bottom: 24px; h1 { margin: 0; } }
    .form-card { padding: 8px; }
    .form-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 0 16px; }
    .full-width { grid-column: 1 / -1; }
    .form-actions { display: flex; justify-content: flex-end; gap: 8px; margin-top: 16px; }
    .error-message { color: var(--mat-sys-error); font-size: 14px; }
  `]
})
export class DeviceForm implements OnInit {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private deviceService = inject(DeviceService);

  form!: FormGroup;
  loading = false;
  errorMessage = '';
  isEdit = false;
  deviceId = '';

  deviceTypes = [
    { value: DeviceType.Smartphone, label: 'Smartphone' },
    { value: DeviceType.Tablet, label: 'Tablet' },
    { value: DeviceType.Laptop, label: 'Laptop' },
    { value: DeviceType.Other, label: 'Other' }
  ];

  ngOnInit(): void {
    this.deviceId = this.route.snapshot.paramMap.get('id') ?? '';
    this.isEdit = !!this.deviceId;

    this.form = this.fb.group({
      name: ['', Validators.required],
      brand: ['', Validators.required],
      model: ['', Validators.required],
      type: [DeviceType.Smartphone, Validators.required],
      operatingSystem: [null],
      osVersion: [null],
      processor: [null],
      ramGb: [null],
      description: [null],
      ...(!this.isEdit && {
        serialNumber: ['', Validators.required],
        assetTag: ['', Validators.required],
        purchasedAtUtc: ['']
      })
    });

    if (this.isEdit) {
      this.deviceService.getById(this.deviceId).subscribe({
        next: (device) => {
          this.form.patchValue({
            name: device.name,
            brand: device.brand,
            model: device.model,
            type: device.type,
            operatingSystem: device.operatingSystem ?? null,
            osVersion: device.osVersion ?? null,
            processor: device.processor ?? null,
            ramGb: device.ramGb ?? null,
            description: device.description ?? null
          });
        }
      });
    }
  }

  onSubmit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }

    this.loading = true;
    this.errorMessage = '';
    const v = this.form.getRawValue();

    const shared = {
      name: v.name, brand: v.brand, model: v.model, type: v.type,
      operatingSystem: v.operatingSystem || undefined,
      osVersion: v.osVersion || undefined,
      processor: v.processor || undefined,
      ramGb: v.ramGb ? Number(v.ramGb) : undefined,
      description: v.description || undefined
    };

    const request$ = this.isEdit
      ? this.deviceService.update(this.deviceId, shared)
      : this.deviceService.create({
          ...shared,
          serialNumber: v.serialNumber,
          assetTag: v.assetTag,
          purchasedAtUtc: v.purchasedAtUtc || undefined
        });

    request$.subscribe({
      next: (device) => this.router.navigate(['/devices', device.id]),
      error: (err) => {
        this.errorMessage = err.error?.message ?? 'An error occurred.';
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.isEdit
      ? this.router.navigate(['/devices', this.deviceId])
      : this.router.navigate(['/devices']);
  }
}
