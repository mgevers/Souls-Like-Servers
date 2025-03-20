import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ToastMessage } from '../../state/toast/toast.reducer';
import { Observable, Subscription, tap } from 'rxjs';
import { Store } from '@ngrx/store';
import { AppState } from '../../state/app.state';
import { selectToast } from '../../state/toast/toast.selector';

@Component({
  selector: 'app-toaster',
  imports: [],
  template: ``,
  styles: ``
})
export class ToasterComponent implements OnDestroy {
  private toast$: Observable<ToastMessage | undefined>;
  private toastSubscription: Subscription;

  constructor (
    private snackBar: MatSnackBar,
    store: Store<AppState>) {
      this.toast$ = store.select(selectToast).pipe(
        tap(toast => this.setSnackBar(toast))
      );

      this.toastSubscription = this.toast$.subscribe();
    }

  ngOnDestroy(): void {
    this.toastSubscription.unsubscribe();
  }

  private setSnackBar(toast: ToastMessage | undefined) {
    if (toast) {
      const toastClass = `${toast.type ?? 'success'}-snackbar`;

      this.snackBar.open(toast.message, toast.action ?? 'Close', {
        horizontalPosition: toast.horizontalPosition ?? 'right',
        verticalPosition: toast.verticalPosition ?? 'bottom',
        duration: toast.durationMilliseconds ?? 3000,
        panelClass: [toastClass],
      });
    } else {
      this.snackBar.dismiss();
    }
  }
}
