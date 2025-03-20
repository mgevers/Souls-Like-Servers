import { 
  Component,
  ComponentRef,
  OnDestroy,
  OnInit,
  output,
  viewChild,
  ViewContainerRef,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';
import { MatDividerModule } from '@angular/material/divider';

import { AddMonsterRequest, SoulsAttributeName } from '../../../services/contracts';
import { AppForm } from '../../../components/forms/form.contracts';
import { SetAttributesFormComponent } from "../../../components/forms/set-attributes-form.component";

export type AttributeData = {
  attributeName: FormControl<SoulsAttributeName | null>
  attributeValue: FormControl<number | null>,
}

export type AddMonsterFormData = {
  monsterId: FormControl<string | null>;
  monsterName: FormControl<string | null>;
  monsterLevel: FormControl<number | null>;
}

@Component({
  selector: 'app-add-monster-form',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatButtonModule,
    MatDialogModule,
    MatSelectModule,
    MatDividerModule,
  ],
  template: `
    <form [formGroup]="monsterForm" (ngSubmit)="onSubmit()">
      <div bp-layout="grid gap:xs cols:12">
        <div bp-layout="col:12@xs">
          <mat-form-field appearance="fill">
            <mat-label>Monster ID</mat-label>
            <input matInput placeholder="Monster ID" formControlName="monsterId">
            <mat-icon matSuffix>key</mat-icon>
          </mat-form-field>
        </div>
        <div bp-layout="col:12@xs col:6@sm">
          <mat-form-field appearance="fill">
            <mat-label>Monster Name</mat-label>
            <input matInput placeholder="Monster Name" formControlName="monsterName">
          </mat-form-field>
        </div>
        <div bp-layout="col:12@xs col:6@sm">
          <mat-form-field appearance="fill">
            <mat-label>Monster Level</mat-label>
            <input matInput placeholder="Monster Level" formControlName="monsterLevel">
          </mat-form-field>
        </div>
        <mat-divider />
        <ng-container #attributesContainer></ng-container>
        <div class="submit-div">
          <button mat-flat-button type="submit" class="submit-button">Create Monster</button>
        </div>
      </div>
    </form>
  `,
  styles: [`
    mat-form-field {
      width: 100%;
    }

    .submit-div {
      padding-top: 50px;
    }

    .submit-button {
      width: 100%;
    }
  `]
})

export class AddMonsterFormComponent implements 
  AppForm<AddMonsterRequest>, OnInit, OnDestroy {
  private containerRef = viewChild('attributesContainer', { read: ViewContainerRef });
  private attributeFormRef: ComponentRef<SetAttributesFormComponent> | undefined;

  monsterForm: FormGroup<AddMonsterFormData>;
  submitted = output<AddMonsterRequest>();

  constructor(private formBuilder: FormBuilder) {
    this.monsterForm = this.formBuilder.group<AddMonsterFormData>({
      monsterId: new FormControl(crypto.randomUUID()),
      monsterName: new FormControl(''),
      monsterLevel: new FormControl(1),
    });
  }

  ngOnInit(): void {
    this.attributeFormRef = this.containerRef()?.createComponent<SetAttributesFormComponent>(SetAttributesFormComponent);
    this.attributeFormRef?.instance.addAttribute('maxHealth', 100);
  }
  ngOnDestroy(): void {
    this.attributeFormRef?.destroy();
  }

  onSubmit(): void {
    const addMonsterRequest: AddMonsterRequest = {
      monsterId: this.monsterForm.value['monsterId']!,
      monsterName: this.monsterForm.value['monsterName']!,
      monsterLevel: this.monsterForm.value['monsterLevel']!,
      attributeSet: this.attributeFormRef!.instance.getAttributes(),
    };

    this.submitted.emit(addMonsterRequest);
  }
}
