import { Component, signal } from '@angular/core';
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

import { SoulsAttributeName, SoulsAttributeSet } from '../../services/contracts';

type AttributeOptions = {
  index: number
  options: SoulsAttributeName[],
}

export type AttributeData = {
  attributeName: FormControl<SoulsAttributeName | null>
  attributeValue: FormControl<number | null>,
}

@Component({
  selector: 'app-set-attributes-form',
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
    <h4>Attributes</h4>
    <ng-container *ngFor="let attForm of attributeForm.controls; let i = index">
      <div bp-layout="grid gap:xs cols:12" [formGroup]="attForm">
        <div bp-layout="col:6@xs">
          <mat-form-field>
            <mat-label>Attribute Name</mat-label>
            <mat-select formControlName="attributeName">
              <mat-option *ngFor="let attName of getAvailableAttributes(attributeOptions(), i)" [value]="attName">{{attName}}</mat-option>
            </mat-select>
          </mat-form-field>
        </div>
        <div bp-layout="col:5@xs">
          <mat-form-field floatLabel="always">
            <mat-label>Attribute Value</mat-label>
            <input matInput type="number" placeholder="0" formControlName="attributeValue"/>
          </mat-form-field>
        </div>
        <div bp-layout="col:1@xs">
          <button mat-icon-button type="button" (click)="removeAttribute(i)">
            <mat-icon>remove_circle_outline</mat-icon>
          </button>
        </div>
      </div>
    </ng-container>
    <button
      mat-stroked-button
      *ngIf="canAddAttribute()"
      class="add-attribute-button"
      type="button"
      (click)="addAttribute()">
      Add Attribute
    </button>
  `,
  styles: [`
    mat-form-field {
      width: 100%;
    }

    .add-attribute-button {
      width: 35%;
    }

    h4 {
      text-align: center;
    }
  `]
})

export class SetAttributesFormComponent {
  private readonly attributeNames: SoulsAttributeName[] = [
    'maxHealth',
    'maxMana',
    'maxStamina',
    'physicalPower',
    'physicalDefense',
  ]

  attributeForm: FormArray<FormGroup<AttributeData>>;
  canAddAttribute = signal<boolean>(true)
  attributeOptions = signal<AttributeOptions[]>([
    {
      index: 0,
      options: this.attributeNames
    },
  ]);

  constructor(private formBuilder: FormBuilder) {
    this.attributeForm = this.formBuilder.array<FormGroup<AttributeData>>([]);
  }

  getAttributes(): SoulsAttributeSet {
    const attributeSet: SoulsAttributeSet = {
      maxHealth: this.getAttributeValueFromForm('maxHealth'),
      maxMana: this.getAttributeValueFromForm('maxMana'),
      maxStamina: this.getAttributeValueFromForm('maxStamina'),
      physicalDefense: this.getAttributeValueFromForm('physicalDefense'),
      physicalPower: this.getAttributeValueFromForm('physicalPower'),
    };

    return attributeSet;
  }

  seedAttributes(attributeSet: SoulsAttributeSet) {
    if (attributeSet.maxHealth) {
      this.addAttribute('maxHealth', attributeSet.maxHealth);
    }
    if (attributeSet.maxMana) {
      this.addAttribute('maxMana', attributeSet.maxMana);
    }
    if (attributeSet.maxStamina) {
      this.addAttribute('maxStamina', attributeSet.maxStamina);
    }
    if (attributeSet.physicalDefense) {
      this.addAttribute('physicalDefense', attributeSet.physicalDefense);
    }
    if (attributeSet.physicalPower) {
      this.addAttribute('physicalPower', attributeSet.physicalPower);
    }
  }

  addAttribute(attributeName?: SoulsAttributeName, attributeValue?: number) {
    const unusedAttributes = this.getUnsetAttributes();

    if (attributeName && !unusedAttributes.includes(attributeName)) {
      throw new Error(`attribute name ${attributeName} already in use`);
    }

    const attributeGroup = this.formBuilder.group<AttributeData>({
      attributeName: new FormControl(unusedAttributes[0]),
      attributeValue: new FormControl(attributeValue ?? 100),
    })

    this.attributeForm.push(attributeGroup);

    const newOptions: AttributeOptions[] = [];
    for (let i = 0; i < this.attributeForm.value.length; i++) {
      newOptions.push({ index: i, options: this.getAttributeOptions(i) });
    }

    this.attributeOptions.set(newOptions);

    if (this.getUnsetAttributes().length === 0) {
      this.canAddAttribute.set(false);
    }
  }

  removeAttribute(index: number) {
    this.attributeForm.removeAt(index);
    this.canAddAttribute.set(true);

    const newOptions: AttributeOptions[] = [];
    for (let i = 0; i < this.attributeForm.value.length; i++) {
      newOptions.push({ index: i, options: this.getAttributeOptions(i) });
    }

    this.attributeOptions.set(newOptions);
  }

  getAvailableAttributes(available: AttributeOptions[], index: number): SoulsAttributeName[] {
    const options = available.filter(s => s.index === index);

    if (options.length === 1) {
      return options[0].options;
    }

    return [];
  }

  private getAttributeOptions(index: number): SoulsAttributeName[] {
    const unsetAttributes = this.getUnsetAttributes();
    const setAttribute = this.attributeForm.value[index].attributeName;

    const options = setAttribute
      ? [...unsetAttributes, setAttribute]
      : unsetAttributes;

    return options.sort((a, b) => a.localeCompare(b));
  }

  private getUnsetAttributes(): SoulsAttributeName[] {
    const setAttributes = this.attributeForm.value
      .flatMap(att => att.attributeName)
      .filter(att => !!att);

    const unusedAttributes = this.attributeNames.filter(att => !setAttributes.includes(att));

    return unusedAttributes;
  }

  private getAttributeValueFromForm(attributeName: SoulsAttributeName): number {
    const attribute = this.attributeForm.value.filter(f => f.attributeName === attributeName);

    if (attribute.length === 1) {
      return attribute[0].attributeValue ?? 0;
    }

    return 0;
  }
}
