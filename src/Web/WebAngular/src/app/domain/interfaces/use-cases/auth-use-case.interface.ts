import { Observable } from "rxjs";
import { GeneralSignInCommand } from "../../commands/general-sign-in.command";
import { BearerTokenViewModel } from "../../view-models/bearer-token.view-model";

export abstract class AuthUseCaseInterface {
	abstract signIn(body: GeneralSignInCommand): Observable<BearerTokenViewModel>;
	abstract refreshToken(refreshToken: string): Observable<BearerTokenViewModel>;
}
