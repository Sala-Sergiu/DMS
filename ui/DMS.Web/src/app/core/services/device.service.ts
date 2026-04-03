import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  Device,
  CreateDeviceRequest,
  UpdateDeviceRequest,
  DeviceListRequest,
  PagedResponse
} from '../models/device.model';
import { AssignDeviceRequest, ReturnDeviceRequest } from '../models/assignment.model';
import { GenerateDescriptionRequest, GenerateDescriptionResponse } from '../models/description.model';

@Injectable({ providedIn: 'root' })
export class DeviceService {
  private readonly apiUrl = '/api/devices';

  constructor(private http: HttpClient) { }

  getPaged(request: DeviceListRequest): Observable<HttpResponse<PagedResponse<Device>>> {
    let params = new HttpParams()
      .set('pageNumber', request.pageNumber.toString())
      .set('pageSize', request.pageSize.toString());

    if (request.searchTerm) params = params.set('searchTerm', request.searchTerm);
    if (request.type != null) params = params.set('type', request.type.toString());
    if (request.status != null) params = params.set('status', request.status.toString());
    if (request.sortBy) params = params.set('sortBy', request.sortBy);
    if (request.sortDescending != null) params = params.set('sortDescending', request.sortDescending.toString());

    return this.http.get<PagedResponse<Device>>(this.apiUrl, { params, observe: 'response' });
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

  assign(deviceId: string, request: AssignDeviceRequest): Observable<Device> {
    return this.http.post<Device>(`${this.apiUrl}/${deviceId}/assign`, request);
  }

  return(deviceId: string, request: ReturnDeviceRequest): Observable<Device> {
    return this.http.post<Device>(`${this.apiUrl}/${deviceId}/return`, request);
  }

  generateDescription(request: GenerateDescriptionRequest): Observable<GenerateDescriptionResponse> {
    return this.http.post<GenerateDescriptionResponse>(`${this.apiUrl}/generate-description`, request);
  }
}
