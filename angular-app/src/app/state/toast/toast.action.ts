import { createAction, props } from '@ngrx/store';
import { ToastMessage } from './toast.reducer';

export const showToast = createAction(
    'Show Toast',
    props<{ toast: ToastMessage }>()
);

export const removeToast = createAction('Remove Toast');
