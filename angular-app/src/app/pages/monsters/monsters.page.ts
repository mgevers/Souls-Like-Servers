import { Component, OnDestroy } from '@angular/core';
import { Store, StoreModule } from '@ngrx/store';
import { 
  catchError,
  filter,
  lastValueFrom,
  map,
  Observable,
  of,
  Subscription,
  tap,
} from 'rxjs';
import { CommonModule } from '@angular/common';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatDialogModule } from '@angular/material/dialog';

import { MonstersCardComponent } from "../../features/monsters/monster-card.component";
import { loadMonsters, removeMonster } from '../../state/monsters/monster.actions';
import { selectMonsters } from '../../state/monsters/monster.selectors';
import { AppState } from '../../state/app.state';
import { showToast } from '../../state/toast/toast.action';
import { ToastMessage } from '../../state/toast/toast.reducer';
import {
  AddMonsterRequest,
  Envelopes,
  IsMonsterEnvelope,
  IsMonsterSuccessEnvelope,
  Monster,
  MonsterEnvelopes,
  UpdateMonsterRequest,
} from '../../services/contracts';
import { PresentationHubClient } from '../../services/presentation-hub';
import { AddMonsterFormComponent } from '../../features/monsters/forms/add-monster-form.component';
import { ModalService } from '../../services/modals';
import { PresentationClient } from '../../services/presentation-client';
import { HttpErrorResponse } from '@angular/common/http';
import { UpdateMonsterFormComponent } from '../../features/monsters/forms/update-monster-form.component';

@Component({
  selector: 'app-monsters-page',
  imports: [
    CommonModule,
    MonstersCardComponent,
    MatGridListModule,
    StoreModule,
    MatDialogModule,
  ],
  template: `
    <div bp-layout="grid gap:xs cols:12">
      <div bp-layout="col:9@xs">
        <h1>
          monsters page!
        </h1>
      </div>
      <div bp-layout="col:3@xs" class="add-monster-div">
        <button mat-raised-button class="add-monster-button" (click)="showCreateMonsterForm()">
          Create Monster
        </button>
      </div>
    </div>
    <div bp-layout="grid gap:sm cols:12">
      <div bp-layout="col:12@xs col:6@sm col:6@md" *ngFor="let monster of monsters$ | async">
        <app-monster-card [monster]="monster" (editClicked)="onEditClicked(monster)" (deleteClicked)="onDeleteClicked($event)" />
      </div>
    </div>
  `,
  styles: [
    `
      .add-monster-div {
        align-self: flex-end;
        padding-bottom: 1em;
      }

      .add-monster-button {
        height:3em !important;
      }
    `
  ]
})
export class MonstersPage implements OnDestroy {
  private monsterEvents$: Observable<MonsterEnvelopes>;
  private monsterEventSubscription: Subscription;

  public monsters$: Observable<Monster[]>

  constructor(
    private store: Store<AppState>,
    private modals: ModalService,
    private presentationClient: PresentationClient,
    hubClient: PresentationHubClient) {
    this.monsters$ = store.select(selectMonsters).pipe(
      map(arr => {
        const sortedArray = [...arr];
        sortedArray.sort((a, b) => a.monsterName.localeCompare(b.monsterName));

        return sortedArray;
      }),
    );

    this.store.dispatch(loadMonsters());

    this.monsterEvents$ = this.handleMonsterEvents(hubClient.events$);
    this.monsterEventSubscription = this.monsterEvents$.subscribe();
  }

  ngOnDestroy(): void {
    this.monsterEventSubscription.unsubscribe();
  }

  onEditClicked(monster: Monster) {
    const modal = this.modals.openForm<UpdateMonsterRequest, UpdateMonsterFormComponent>(
      'Update Monster', 
      UpdateMonsterFormComponent,
      {
        monster
      });

    modal.componentInstance.onSubmit.subscribe(async (data: UpdateMonsterRequest) => {
      const request = this.presentationClient.updateMonster(data).pipe(
        tap(() => this.modals.closeForms()),
        catchError((error: HttpErrorResponse) => {
          const toast: ToastMessage = {
            message: error.error,
            type: 'failure'
          }
          this.store.dispatch(showToast({ toast }));

          return of(error);
        }),
      );

      await lastValueFrom(request);
    })
  }

  onDeleteClicked(id: string) {
    this.store.dispatch(removeMonster({ id }));
  }

  showCreateMonsterForm(): void {
    const modal = this.modals.openForm<AddMonsterRequest, AddMonsterFormComponent>('Add Monster', AddMonsterFormComponent);

    modal.componentInstance.onSubmit.subscribe(async (data: AddMonsterRequest) => {
      const request = this.presentationClient.addMonster(data).pipe(
        tap(() => this.modals.closeForms()),
        catchError((error: HttpErrorResponse) => {
          const toast: ToastMessage = {
            message: error.error,
            type: 'failure'
          }
          this.store.dispatch(showToast({ toast }));

          return of(error);
        }),
      );

      await lastValueFrom(request);
    })
  }

  private handleMonsterEvents(events$: Observable<Envelopes>): Observable<MonsterEnvelopes> {
    return events$.pipe(
      filter(envelope => IsMonsterEnvelope(envelope)),
      tap(envelope => this.showToastForEvent(envelope)),
    );
  }

  private showToastForEvent(envelope: MonsterEnvelopes): void {
    const isSuccess = IsMonsterSuccessEnvelope(envelope);

    const toast: ToastMessage = {
      message: envelope.eventType,
      type: isSuccess ? 'success' : 'failure',
      durationMilliseconds: isSuccess ? 3_000 : 10_000,
    };

    this.store.dispatch(showToast({ toast }));
  }
}
