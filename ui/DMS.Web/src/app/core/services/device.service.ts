import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  Device, PagedResult, CreateDeviceRequest, UpdateDeviceRequest, DeviceType
} from '../models/device.model';

export interface DeviceListParams {
  searchTerm?: string;
  type?: number;
  status?: number;
  sortBy?: string;
  sortDescending?: boolean;
  pageNumber?: number;
  pageSize?: number;
}

// 1=Smartphone, 2=Tablet, 3=Laptop, 99=Other
const DEVICE_TYPE_LABEL: Record<DeviceType, string> = {
  [DeviceType.Smartphone]: 'Smartphone',
  [DeviceType.Tablet]: 'Tablet',
  [DeviceType.Laptop]: 'Laptop',
  [DeviceType.Other]: 'Other'
};

@Injectable({ providedIn: 'root' })
export class DeviceService {
  private http = inject(HttpClient);
  private readonly apiUrl = '/api/devices';

  getAll(params: DeviceListParams = {}): Observable<PagedResult<Device>> {
    let httpParams = new HttpParams();
    if (params.searchTerm) httpParams = httpParams.set('searchTerm', params.searchTerm);
    if (params.type != null) httpParams = httpParams.set('type', params.type);
    if (params.status != null) httpParams = httpParams.set('status', params.status);
    if (params.sortBy) httpParams = httpParams.set('sortBy', params.sortBy);
    if (params.sortDescending != null) httpParams = httpParams.set('sortDescending', params.sortDescending);
    if (params.pageNumber != null) httpParams = httpParams.set('pageNumber', params.pageNumber);
    if (params.pageSize != null) httpParams = httpParams.set('pageSize', params.pageSize);

    return this.http.get<PagedResult<Device>>(this.apiUrl, { params: httpParams });
  }

  getById(id: string): Observable<Device> {
    return this.http.get<Device>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateDeviceRequest): Observable<Device> {
    return this.http.post<Device>(this.apiUrl, request);
  }

  update(id: string, request: UpdateDeviceRequest): Observable<Device> {
    return this.http.put<Device>(`${this.apiUrl}/${id}`, request);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  assignToSelf(id: string, notes?: string): Observable<Device> {
    return this.http.post<Device>(`${this.apiUrl}/${id}/assign`, { notes });
  }

  unassignFromSelf(id: string): Observable<Device> {
    return this.http.post<Device>(`${this.apiUrl}/${id}/unassign`, {});
  }

  generateDescription(request: {
    name: string;
    brand: string;
    model: string;
    type: DeviceType;
  }): Observable<{ description: string }> {
    return this.http.post<{ description: string }>(`${this.apiUrl}/generate-description`, {
      name: request.name,
      brand: request.brand,
      model: request.model,
      type: DEVICE_TYPE_LABEL[request.type] ?? 'Other'
    });
  }
}
