import { createReducer, on } from '@ngrx/store';
import {
    MatSnackBarHorizontalPosition,
    MatSnackBarVerticalPosition,
  } from '@angular/material/snack-bar';
import { removeToast, showToast } from './toast.action';

export type ToastType = 'success' | 'warning' | 'failure';

export type ToastMessage = {
    message: string,
    type?: ToastType,
    action?: string,
    durationMilliseconds?: number,
    horizontalPosition?: MatSnackBarHorizontalPosition,
    verticalPosition?: MatSnackBarVerticalPosition,
}

export type ToastState = {
    message?: ToastMessage,
}

export const initialState: ToastState = {}

export const toastReducer = createReducer(
    initialState,
    on(showToast, (state, { toast }) => ({        
        ...state,
        message: {...toast},
    })),
    on(removeToast, (state) => ({
        ...state,
        message: undefined,
    })),
);
