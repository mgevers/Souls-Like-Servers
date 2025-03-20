import { createAction, props } from '@ngrx/store';

import { AddMonsterRequest, Monster, UpdateMonsterRequest } from '../../services/contracts';


export const addMonster = createAction(
    'Add Monster',
    props<{ request: AddMonsterRequest }>()
);

export const addMonsterSuccess = createAction(
    'Add Monster Success',
    props<{ monster: Monster }>()
);

export const addMonsterFailure = createAction(
    'Add Monster Failure',
    props<{ error: string }>()
);

export const updateMonster = createAction(
    'Update Monster',
    props<{ request: UpdateMonsterRequest }>()
);

export const updateMonsterSuccess = createAction(
    'Update Monster Success',
    props<{ monster: Monster }>()
);

export const updateMonsterFailure = createAction(
    'Update Monster Success',
    props<{ error: string }>()
);

export const removeMonster = createAction(
    'Remove Monster',
    props<{ id: string }>()
);

export const removeMonsterSuccess = createAction(
    'Remove Monster Success',
    props<{ id: string }>()
);

export const removeMonsterFailure = createAction(
    'Remove Monster Success',
    props<{ error: string }>()
);

export const loadMonsters = createAction('Load Monsters');

export const loadMonstersSuccess = createAction(
    'Load Monsters Success',
    props<{ monsters: Monster[] }>(),
)

export const loadMonstersFailure = createAction(
    'Load Monsters Failure',
    props<{ error: string }>(),
)
