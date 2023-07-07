import { UserRole } from "../../enums/user-role";

export interface CreateUserCommand {
	name: string;
	email: string;
	tempPassword: string;
	userRole: UserRole
}
