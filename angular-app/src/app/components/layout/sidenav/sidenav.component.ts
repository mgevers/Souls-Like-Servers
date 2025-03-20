import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';

import { AuthButtonComponent } from "../../../features/auth/auth.button";

export type NavbarItem = {
  icon: string;
  label: string;
  route?: string; 
};

@Component({
  selector: 'app-sidenav',
  imports: [
    CommonModule,
    MatListModule,
    MatIconModule,
    RouterModule,
    AuthButtonComponent
],
  template: `
    <mat-nav-list>
      <a 
        mat-list-item
        class="navbar-item"
        *ngFor="let item of navbarItems()"
        [routerLink]="item.route"
        routerLinkActive="selected-navbar-item"
        [routerLinkActiveOptions]="{ exact: true }"
        #rla="routerLinkActive"
        [activated]="rla.isActive" >
        <mat-icon matListItemIcon>{{ item.icon }}</mat-icon>
        <span matListItemTitle>{{ item.label }}</span>
      </a>
    </mat-nav-list>
    <div class="auth-button-box">
      <app-auth-button />
    </div>
  `,
  styles: [
    `
      :host * {
        transition: all 500ms ease-in-out
      }

      .navbar-item {
        border-radius: 0px;
        border-left: 5px solid;
        border-left-color: rgba(0, 0, 0, 0);
      }

      .selected-navbar-item {        
        border-left-color: blue;
        background: rgba(0, 0, 0, 0.05);
      }

      .auth-button-box {
        clear:both;
        position:relative;
      }
    `
  ]
})
export class SidenavComponent {
  navbarItems = signal<NavbarItem[]>([
    {
      icon: 'home',
      label: 'Home',
      route: 'home',
    },
    {
      icon: 'gavel',
      label: 'Items',
      route: 'items',
    },
    {
      icon: 'nordic_walking',
      label: 'Monsters',
      route: 'monsters',
    },
    {
      icon: 'admin_panel_settings',
      label: 'Admin',
      route: 'admin',
    },
  ]);
}
