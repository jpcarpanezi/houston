import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import 'codemirror/mode/shell/shell';

platformBrowserDynamic().bootstrapModule(AppModule)
	.catch(err => console.error(err));
