import { UserRole } from "../enums/user-role";

export interface UserSessionViewModel {
	name: string;
	email: string;
	role: UserRole;
}
