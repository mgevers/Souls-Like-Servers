import { 
  Component,
  ComponentRef,
  Input,
  input,
  OnDestroy,
  OnInit,
  output,
  viewChild,
  ViewContainerRef,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormArray,
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

import { Monster, SoulsAttributeName, UpdateMonsterRequest } from '../../../services/contracts';
import { AppForm } from '../../../components/forms/form.contracts';
import { SetAttributesFormComponent } from "../../../components/forms/set-attributes-form.component";

export type AttributeData = {
  attributeName: FormControl<SoulsAttributeName | null>
  attributeValue: FormControl<number | null>,
}

export type UpdateMonsterFormData = {
  monsterId: FormControl<string | null>;
  monsterName: FormControl<string | null>;
  monsterLevel: FormControl<number | null>;
}

@Component({
  selector: 'app-update-monster-form',
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
          <button mat-flat-button type="submit" class="submit-button">Update Monster</button>
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

export class UpdateMonsterFormComponent implements 
  AppForm<UpdateMonsterRequest>, OnInit, OnDestroy {
  private containerRef = viewChild('attributesContainer', { read: ViewContainerRef });
  private attributeFormRef: ComponentRef<SetAttributesFormComponent> | undefined;
  private _monster: Monster | undefined;

  monsterForm: FormGroup<UpdateMonsterFormData>;

  @Input() set monster(value: Monster) {
    this._monster = value;
  }

  submitted = output<UpdateMonsterRequest>();

  constructor(private formBuilder: FormBuilder) {
    this.monsterForm = this.formBuilder.group<UpdateMonsterFormData>({
      monsterId: new FormControl(this._monster?.id ?? ''),
      monsterName: new FormControl(''),
      monsterLevel: new FormControl(1),
    });
  }

  ngOnInit(): void {
    this.monsterForm.setValue({
      monsterId: this._monster?.id ?? '',
      monsterLevel: this._monster?.monsterLevel ?? 1,
      monsterName: this._monster?.monsterName ?? ''
    });

    this.attributeFormRef = this.containerRef()?.createComponent<SetAttributesFormComponent>(SetAttributesFormComponent);
    this.attributeFormRef?.instance.seedAttributes(this._monster!.attributeSet);
  }

  ngOnDestroy(): void {
    this.attributeFormRef?.destroy();
  }

  onSubmit(): void {
    const updateMonsterRequest: UpdateMonsterRequest = {
      monsterId: this.monsterForm.value['monsterId']!,
      monsterName: this.monsterForm.value['monsterName']!,
      monsterLevel: this.monsterForm.value['monsterLevel']!,
      attributeSet: this.attributeFormRef!.instance.getAttributes(),
    };
    
    this.submitted.emit(updateMonsterRequest);
  }
}
