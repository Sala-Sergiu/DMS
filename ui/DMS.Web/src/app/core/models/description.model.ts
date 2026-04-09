import { DeviceType } from './device-type.enum';

export interface GenerateDescriptionRequest {
  name: string;
  brand: string;
  model: string;
  type: DeviceType;
  operatingSystem?: string;
  processor?: string;
  ramGb?: number;
}

export interface GenerateDescriptionResponse {
  description: string;
}
