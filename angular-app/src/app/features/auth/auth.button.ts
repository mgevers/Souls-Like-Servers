import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { MatIconModule } from "@angular/material/icon";
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-auth-button',
  imports: [ CommonModule, MatIconModule ],
  template: `
    <ng-container *ngIf="auth.isAuthenticated$ | async; else loggedOut">
      <div class="button-container">
        <button mat-fab extended  (click)="auth.logout({ logoutParams: { returnTo: document.location.origin } })">
          <mat-icon>person_outline</mat-icon>
          Log out
        </button>
      </div>
    </ng-container>

    <ng-template #loggedOut>
      <div class="button-container">
        <button mat-fab extended (click)="auth.loginWithRedirect()">
          <mat-icon>person_outline</mat-icon>
          Log In
        </button>
      </div>
    </ng-template>
  `,
  styles: [`
    .button-contianer {
      display: flex;
      justify-content: center;
      text-align: center;
      vertical-align: middle;
    }

    button {
      width: 100%;
    }  
  `]
})
export class AuthButtonComponent {
  constructor(
    @Inject(DOCUMENT) public document: Document,
    public auth: AuthService) {}
}