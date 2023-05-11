import { Observable } from "rxjs";
import { BearerTokenViewModel } from "../../view-models/bearer-token.view-model";
import { GeneralSignInCommand } from "../../commands/general-sign-in.command";

export abstract class AuthRepositoryInterface {
	abstract signIn(body: GeneralSignInCommand): Observable<BearerTokenViewModel>;
	abstract refreshToken(refreshToken: string): Observable<BearerTokenViewModel>;
}
