import { UserRole } from "../enums/user-role";

export interface UserViewModel {
	id: string;
	email: string;
	name: string;
	active: boolean;
	userRole: UserRole;
}
