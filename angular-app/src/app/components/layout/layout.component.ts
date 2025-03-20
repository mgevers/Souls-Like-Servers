import { Component, computed, OnInit, Renderer2, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSlideToggleModule } from "@angular/material/slide-toggle";

import { SidenavComponent } from "./sidenav/sidenav.component";
import { ToasterComponent } from "../toaster/toaster.component";

@Component({
  selector: 'app-layout',
  imports: [
    RouterOutlet,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatSidenavModule,
    SidenavComponent,
    MatSlideToggleModule,
    ToasterComponent
],
  template: `
    <mat-toolbar class="mat-elevation-z3">
      <button mat-icon-button (click)="isCollapsed.set(!isCollapsed())">
        <mat-icon>menu</mat-icon>
      </button>
      <div class="center-flex">
        <h1>Souls Like</h1>
      </div>
      <div class="floating-right">
        <mat-icon>light_mode</mat-icon>
        <mat-slide-toggle [checked]="isDarkMode()" (change)="toggleTheme($event.checked)">
        </mat-slide-toggle>
        <mat-icon>dark_mode</mat-icon>
      </div>
    </mat-toolbar>
    <mat-sidenav-container>
      <mat-sidenav opened mode="side" [style.width]="sidenavWidth()">
        <app-sidenav />
      </mat-sidenav>
      <mat-sidenav-content class="content" [style.margin-left]="sidenavWidth()">
        <router-outlet />
      </mat-sidenav-content>
    </mat-sidenav-container>
    <app-toaster />
  `,
  styles: [
    `
      .content {
        padding: 24px;
      }

      mat-toolbar {
        position: relative;
        z-index: 5;
      }

      mat-sidenav-container {
        height: calc(100vh - 64px);
      }

      mat-sidenav {
        box-shadow: 0px 0px 5px 2px rgba(0, 0, 0, 0.4);
      }

      mat-sidenav,
      mat-sidenav-content {
        transition: all 500ms ease-in-out
      }

      .center-flex {
        display: flex;
        justify-content: center; /* Centers horizontally */
        width: 100vh;
      }

      .floating-right {
        position: fixed;
        margin-right: 1em;
        margin-top: .5em;
        right: 0;
        top: 0;
      }

    `,
  ]
})
export class LayoutComponent implements OnInit { 
  constructor(private renderer: Renderer2) {}
  isDarkMode = signal(true);
  isCollapsed = signal(false);
  sidenavWidth = computed(() => this.isCollapsed() ? '65px' : '250px');

  ngOnInit(): void {
    const savedTheme = this.loadThemeName();

    this.isDarkMode.set(savedTheme === 'dark');
    this.renderer.setAttribute(document.documentElement, 'data-theme', savedTheme);
  }

  toggleTheme(isDarkMode: boolean): void {
    const theme = isDarkMode ? 'dark' : 'light';
    localStorage.setItem('theme', theme);
    
    this.isDarkMode.set(theme === 'dark');
    this.renderer.setAttribute(document.documentElement, 'data-theme', theme);
  }

  private loadThemeName(): "dark" | "light" {
    return localStorage.getItem('theme')
      || window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
  }
}
