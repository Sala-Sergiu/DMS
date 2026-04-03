import { UserRole } from './user-role.enum';

export interface User {
  id: string;
  fullName: string;
  email: string;
  role: UserRole;
  location: string | null;
  isActive: boolean;
  createdAtUtc: string;
}
