import { Component, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from './state/app.state';
import { filter, Observable, Subscription, tap } from 'rxjs';

import { 
  Envelopes,
  IsMonsterSuccessEnvelope,
  Monster,
  MonsterAddedEvent,
  MonsterEnvelopes,
  MonsterUpdatedEvent,
} from './services/contracts';
import { LayoutComponent } from './components/layout/layout.component';
import { PresentationHubClient } from './services/presentation-hub';
import { addMonsterSuccess, removeMonsterSuccess, updateMonsterSuccess } from './state/monsters/monster.actions';

@Component({
  selector: 'app-root',
  imports: [LayoutComponent],
  template: `<app-layout />`,
  styles: [],
})
export class AppComponent implements OnDestroy {
  private dataSyncEvents$: Observable<Envelopes>;
  private dataSyncSubscription: Subscription;

  constructor(
    private store: Store<AppState>,
    hubClient: PresentationHubClient) {
    this.dataSyncEvents$ = this.handleDataSyncEvents(hubClient.events$);
    this.dataSyncSubscription = this.dataSyncEvents$.subscribe();
  }

  ngOnDestroy(): void {
    this.dataSyncSubscription.unsubscribe();
  }

  private handleDataSyncEvents(events$: Observable<Envelopes>): Observable<Envelopes> {
    return events$.pipe(
      filter(envelope => this.isDataUpdateEvent(envelope)),
      tap(envelope => this.updateStoreForEvent(envelope)),
    )
  }

  private isDataUpdateEvent(envelope: Envelopes) {
    return IsMonsterSuccessEnvelope(envelope);
  }

  private updateStoreForEvent(envelope: MonsterEnvelopes): void {
    switch (envelope.eventType) {
      case "MonsterAddedEvent":
        this.store.dispatch(addMonsterSuccess({ monster: this.getMonsterFromEvent(envelope.payload) }));
        break;
      case "MonsterRemovedEvent":
        this.store.dispatch(removeMonsterSuccess({ id: envelope.payload.monsterId }));
        break;
      case "MonsterUpdatedEvent":
        this.store.dispatch(updateMonsterSuccess({ monster: this.getMonsterFromEvent(envelope.payload) }));
        break;
    }
  }

  private getMonsterFromEvent(event: MonsterAddedEvent | MonsterUpdatedEvent): Monster {
    return {
      id: event.monsterId,
      attributeSet: event.attributeSet,
      monsterName: event.monsterName.value,
      monsterLevel: event.monsterLevel.value,
    };
  }
}
