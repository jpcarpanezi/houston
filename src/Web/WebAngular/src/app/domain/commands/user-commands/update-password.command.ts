export interface UpdatePasswordCommand {
	userId: string;
	oldPassword?: string;
	newPassword: string;
}
