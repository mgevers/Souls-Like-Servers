import { Injectable, OnDestroy } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';

import { environment } from "../../environments/environment";
import { AddMonsterRequest, Envelopes } from './contracts';

@Injectable({
  providedIn: 'root',
})
export class PresentationHubClient implements OnDestroy {
  private readonly evenSource$ = new Subject<Envelopes>;
  private hubConnection: signalR.HubConnection;

  events$: Observable<Envelopes> = this.evenSource$;

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.presentationUrl}/presentation-client`)
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.startConnection();

    this.hubConnection.on('PushEnvelope', (event: Envelopes) => {
      this.evenSource$.next(event);
    });
  }

  addMonster(request: AddMonsterRequest): Promise<void> {
    if (this.hubConnection.state !== signalR.HubConnectionState.Connected) {
      throw new Error(`cannot invoke server methods in the connection state: ${this.hubConnection.state}`);
    }

    return this.hubConnection.invoke<void>('AddMonster', request);
  }

  ngOnDestroy(): void {
    this.stopConnection();
  }

  private startConnection(): void {
    this.hubConnection.start();
  }

  private stopConnection(): void {
    this.hubConnection.stop();
  }
}
