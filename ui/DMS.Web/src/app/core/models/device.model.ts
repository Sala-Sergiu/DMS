import { DeviceType } from './device-type.enum';
import { DeviceStatus } from './device-status.enum';

export interface ActiveAssignment {
  assignmentId: string;
  userId: string;
  userFullName: string;
  assignedAtUtc: string;
}

export interface Device {
  id: string;
  name: string;
  serialNumber: string;
  assetTag: string;
  brand: string;
  model: string;
  type: DeviceType;
  status: DeviceStatus;
  purchasedAtUtc: string | null;
  createdAtUtc: string;
  updatedAtUtc: string;
  activeAssignment: ActiveAssignment | null;
}

export interface CreateDeviceRequest {
  name: string;
  serialNumber: string;
  assetTag: string;
  brand: string;
  model: string;
  type: DeviceType;
  purchasedAtUtc: string | null;
}

export interface UpdateDeviceRequest {
  name: string;
  brand: string;
  model: string;
  type: DeviceType;
}

export interface DeviceListRequest {
  searchTerm?: string;
  type?: DeviceType;
  status?: DeviceStatus;
  sortBy?: string;
  sortDescending?: boolean;
  pageNumber: number;
  pageSize: number;
}

export interface PagedResponse<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}
