export interface UserAssignedDevice {
  assignmentId: string;
  deviceId: string;
  deviceName: string;
  brand: string;
  model: string;
  assignedAtUtc: string;
}

export interface User {
  id: string;
  fullName: string;
  email: string;
  location?: string;
  createdAtUtc: string;
  activeAssignments: UserAssignedDevice[];
}
