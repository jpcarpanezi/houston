import { UserRole } from "../enums/user-role";

export interface UserViewModel {
	email: string;
	name: string;
	userRole: UserRole;
}
