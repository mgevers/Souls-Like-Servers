import { createSelector } from "@ngrx/store";

import { AppState } from "../app.state";
import { ToastState } from "./toast.reducer";

export const selectToastState = (state: AppState) => state.toast;
export const selectToast = createSelector(selectToastState, (state: ToastState) => state.message);
