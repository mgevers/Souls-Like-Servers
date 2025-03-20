import { createSelector } from "@ngrx/store";

import { AppState } from "../app.state";
import { MonsterState } from "./monster.reducer";

export const selectMonstersState = (state: AppState) => state.monsters;
export const selectMonsters = createSelector(selectMonstersState, (state: MonsterState) => state.monsters);
