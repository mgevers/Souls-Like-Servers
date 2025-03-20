import { ApplicationConfig, provideZoneChangeDetection, isDevMode } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { authHttpInterceptorFn, provideAuth0 } from '@auth0/auth0-angular';

import { environment as env } from '../environments/environment';

import { routes } from './app.routes';
import { monsterReducer } from './state/monsters/monster.reducer';
import { MonsterEffects } from './state/monsters/monster.effects';
import { toastReducer } from './state/toast/toast.reducer';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptors([authHttpInterceptorFn])),
    provideStore({ monsters: monsterReducer, toast: toastReducer }),
    provideEffects(MonsterEffects),
    provideStoreDevtools({ maxAge: 25, logOnly: !isDevMode() }),
    provideAuth0({
      ...env.auth,
      httpInterceptor: {
        ...env.httpInterceptor,
      },
    })
  ]
};
