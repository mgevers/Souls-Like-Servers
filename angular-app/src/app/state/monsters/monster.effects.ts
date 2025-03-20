import { inject, Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, from, map, of, switchMap } from "rxjs";

import { PresentationClient } from "../../services/presentation-client";
import { 
    addMonster, 
    addMonsterFailure,
    addMonsterSuccess,
    loadMonsters,
    loadMonstersFailure,
    loadMonstersSuccess,
    removeMonster,
    removeMonsterFailure,
    removeMonsterSuccess,
} from "./monster.actions";

@Injectable()
export class MonsterEffects {
    private actions$ = inject(Actions);
    private presentationClient = inject(PresentationClient);

    loadMonsters$ = createEffect(() => {
        return this.actions$.pipe(
            ofType(loadMonsters),
            switchMap(() => {
                return from(this.presentationClient.getMonsters()).pipe(
                    map(monsters => loadMonstersSuccess({ monsters })),
                    catchError(error => of(loadMonstersFailure({ error })))
                );
            })
        );
    });

    addMonster$ = createEffect(() => {
        return this.actions$.pipe(
            ofType(addMonster),
            switchMap(payload => {
                return from(this.presentationClient.addMonster(payload.request)).pipe(
                    map(monster => addMonsterSuccess({ monster })),
                    catchError(error => of(addMonsterFailure({ error })))
                );
            })
        );
    });

    removeMonster$ = createEffect(() => {
        return this.actions$.pipe(
            ofType(removeMonster),
            switchMap(payload => {
                return from(this.presentationClient.removeMonster(payload.id)).pipe(
                    map(id => removeMonsterSuccess({ id })),
                    catchError(error => of(removeMonsterFailure({ error })))
                );
            })
        );
    });
}
