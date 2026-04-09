import { DeviceType } from './device-type.enum';
import { DeviceStatus } from './device-status.enum';

export { DeviceType } from './device-type.enum';
export { DeviceStatus } from './device-status.enum';

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
  operatingSystem?: string;
  osVersion?: string;
  processor?: string;
  ramGb?: number;
  description?: string;
  purchasedAtUtc?: string;
  createdAtUtc: string;
  updatedAtUtc: string;
  activeAssignment?: ActiveAssignment;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}

export interface CreateDeviceRequest {
  name: string;
  serialNumber: string;
  assetTag: string;
  brand: string;
  model: string;
  type: DeviceType;
  operatingSystem?: string;
  osVersion?: string;
  processor?: string;
  ramGb?: number;
  description?: string;
  purchasedAtUtc?: string;
}

export interface UpdateDeviceRequest {
  name: string;
  brand: string;
  model: string;
  type: DeviceType;
  operatingSystem?: string;
  osVersion?: string;
  processor?: string;
  ramGb?: number;
  description?: string;
}
