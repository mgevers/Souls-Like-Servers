import { Component, EventEmitter, Input, Output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { Monster } from '../../services/contracts';

@Component({
  selector: 'app-monster-card',
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
  ],
  template: `
  <mat-card *ngIf="loadedMonster(); else noContent">
    <mat-card-header>
      <div bp-layout="inline gap:lg inline:stretch">
        <div>
          <mat-card-title>{{ loadedMonster()?.monsterName }}</mat-card-title>
        </div>
        <div bp-layout="inline gap:xs inline:stretch">
          <mat-icon class="mirror">align_vertical_bottom</mat-icon>
          <mat-card-title>Level: {{ loadedMonster()?.monsterLevel }}</mat-card-title>
        </div>
      </div>
    </mat-card-header>
    <mat-card-content>
      <div *ngIf="loadedMonster()?.attributeSet?.maxHealth" bp-layout="inline gap:lg block:center">
        <div>
          <mat-icon>medical_services</mat-icon>
        </div>
        <div>     
          <h4>Health: {{ loadedMonster()?.attributeSet?.maxHealth }}</h4>
        </div>
      </div>
      <div *ngIf="loadedMonster()?.attributeSet?.maxMana" bp-layout="inline gap:lg block:center">
        <div>
          <mat-icon>book</mat-icon>
        </div>
        <div>     
          <h4>Mana: {{ loadedMonster()?.attributeSet?.maxMana }}</h4>
        </div>
      </div>
      <div *ngIf="loadedMonster()?.attributeSet?.maxStamina" bp-layout="inline gap:lg block:center">
        <div>
          <mat-icon>energy_savings_leaf</mat-icon>
        </div>
        <div>     
          <h4>Stamina: {{ loadedMonster()?.attributeSet?.maxStamina }}</h4>
        </div>
      </div>
      <div *ngIf="loadedMonster()?.attributeSet?.physicalPower" bp-layout="inline gap:lg block:center">
        <div>
          <mat-icon>power</mat-icon>
        </div>
        <div>     
          <h4>Physical Power: {{ loadedMonster()?.attributeSet?.physicalPower }}</h4>
        </div>
      </div>
      <div *ngIf="loadedMonster()?.attributeSet?.physicalDefense" bp-layout="inline gap:lg block:center">
        <div>
          <mat-icon>shield</mat-icon>
        </div>
        <div>     
          <h4>Physcial Defense: {{ loadedMonster()?.attributeSet?.physicalDefense }}</h4>
        </div>
      </div>
    </mat-card-content>
    <mat-card-actions>
      <div bp-layout="inline gap:lg inline:stretch" style="height: 100%">
        <div>
          <button mat-raised-button (click)="editClicked.emit(loadedMonster()?.id)" >Edit</button>
        </div>
        <div>
          <button mat-raised-button (click)="deleteClicked.emit(loadedMonster()?.id)" >Delete</button>
        </div>
      </div>
    </mat-card-actions>
  </mat-card>

  <ng-template #noContent>
    <p>monster not loaded</p>
  </ng-template>
  `,
  styles: [
    `
      button {
        margin: 10px;
      }
    `
  ]
})
export class MonstersCardComponent {
  loadedMonster = signal<Monster | undefined>(undefined);

  @Input() set monster(value: Monster) {
    this.loadedMonster.set(value);
  }

  @Output() editClicked = new EventEmitter<string>();
  @Output() deleteClicked = new EventEmitter<string>();
}
