import { createReducer, on } from '@ngrx/store';

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
    updateMonster,
    updateMonsterFailure,
    updateMonsterSuccess,
} from './monster.actions';
import { Monster } from '../../services/contracts';

export type Status = 'loading' | 'error' | 'success';

export type QueryState = {
    error?: string;
    status?: Status
}

export type SaveState = {
    error?: string;
    status?: Status
}

export type MonsterState = {
    monsters: Monster[],
    queryState: QueryState,
    saveState: SaveState,
}

export const initialState: MonsterState = {
    monsters: [],
    queryState: { },
    saveState: { }
};

export const monsterReducer = createReducer(
    initialState,
    on(addMonster, (state) => ({
        ...state,
        saveState: { 
            status: 'loading' as Status,
            error: undefined,
        }
    })),
    on(addMonsterSuccess, (state, { monster }) => ({
        ...state,
        monsters: [...state.monsters.filter(m => m.id !== monster.id), monster],
        saveState: { 
            status: 'success' as Status,
            error: undefined,
        }
    })),
    on(addMonsterFailure, (state, { error }) => ({
        ...state,
        saveState: { 
            status: 'error' as Status,
            error: error,
        }
    })),

    on(updateMonster, (state) => ({
        ...state,
        saveState: { 
            status: 'loading' as Status,
            error: undefined,
        }
    })),
    on(updateMonsterSuccess, (state, { monster }) => ({
        ...state,
        monsters: [...state.monsters.filter(m => m.id !== monster.id), monster],
        saveState: { 
            status: 'success' as Status,
            error: undefined,
        }
    })),
    on(updateMonsterFailure, (state, { error }) => ({
        ...state,
        saveState: { 
            status: 'error' as Status,
            error: error,
        }
    })),

    on(removeMonster, (state) => ({
        ...state,
        saveState: { 
            status: 'loading' as Status,
            error: undefined,
        }
    })),
    on(removeMonsterSuccess, (state, { id }) => ({
        ...state,
        monsters: [...state.monsters.filter(monster => monster.id !== id)],
        saveState: { 
            status: 'success' as Status,
            error: undefined,
        }
    })),
    on(removeMonsterFailure, (state, { error }) => ({
        ...state,
        saveState: { 
            status: 'error' as Status,
            error: error,
        }
    })),

    on(loadMonsters, (state) => ({
        ...state,
        status: 'loading' as Status,
    })),
    on(loadMonstersSuccess, (state, { monsters }) => ({
        ...state,
        monsters: monsters,
        error: undefined,
        status: 'success' as Status,
    })),
    on(loadMonstersFailure, (state, { error }) => ({
        ...state,
        status: 'error' as Status,
        error: error,
    })),
)
