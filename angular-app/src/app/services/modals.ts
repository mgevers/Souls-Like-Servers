import { ComponentType } from "@angular/cdk/portal";
import {
    Component,
    ComponentRef,
    Injectable,
    OnDestroy,
    OnInit,
    output,
    signal,
    viewChild,
    ViewContainerRef,
} from "@angular/core";
import {
    MatDialog,
    MatDialogConfig,
    MatDialogRef,
    MatDialogModule,
} from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';

import { AppForm } from "../components/forms/form.contracts";

@Injectable({
    providedIn: 'root',
})
export class ModalService {
    private dialogRef: MatDialogRef<any, any> | undefined;

    constructor(private dialog: MatDialog) { }

    openForm<FormData, T extends AppForm<FormData>, D = any, R = any>(
        title: string,
        template: ComponentType<T>,
        inputs?: any,
        config?: MatDialogConfig<D>): MatDialogRef<ModalForm<FormData, T>, R> {
        this.dialogRef = this.dialog.open(ModalForm<FormData, T>, config);
        this.dialogRef.componentInstance.modalTitle.set(title);
        this.dialogRef.componentInstance.modalComponent.set(template);
        this.dialogRef.componentInstance.componentInputs.set(inputs);

        return this.dialogRef;
    }

    closeForms() {
        this.dialogRef?.componentInstance.ngOnDestroy();
        this.dialog.closeAll();

        this.dialogRef = undefined;
    }
}

@Component({
    selector: 'app-modal-popup',
    imports: [MatDialogModule, MatIconModule, MatDividerModule],
    template: `
        <div class="modal-wrapper">
        <div bp-layout="grid gap:xs cols:12">
            <div bp-layout="col:9@xs">
                <h2 class="dialog-title">{{modalTitle()}}</h2>
            </div>            
            <div bp-layout="col:3@xs">
                <mat-dialog-actions>
                    <button mat-icon-button mat-dialog-close><mat-icon>close</mat-icon></button>
                </mat-dialog-actions>
            </div>
        </div>
        <mat-divider></mat-divider>
        <mat-dialog-content class="form-container">
            <ng-container #container>
            </ng-container>
        </mat-dialog-content>
        </div>
    `,
    styles: [`
        .dialog-title {
            text-align: left;
            padding-left: 2em;
        }

        .modal-wrapper {
            width: 80vw;
        }

        .form-container {
            padding: 30px;
        }
    `]
})
export class ModalForm<F, T extends AppForm<F>> implements OnInit, OnDestroy {
    private containerRef = viewChild('container', { read: ViewContainerRef });
    private componentRef: ComponentRef<T> | undefined;

    modalTitle = signal<String | undefined>(undefined);
    modalComponent = signal<ComponentType<T> | undefined>(undefined);
    componentInputs = signal<any | undefined>(undefined);

    onSubmit = output<F>();

    ngOnInit(): void {
        const componentType = this.modalComponent();
        if (!componentType) {
            throw new Error("component type cannot be undefined");
        }

        this.componentRef = this.containerRef()?.createComponent<T>(componentType);
        this.componentRef?.instance.submitted.subscribe(data => this.onSubmit.emit(data));

        if (this.componentInputs()) {
            const keys = Object.keys(this.componentInputs());

            keys.forEach(key => {
                this.componentRef?.setInput(key, this.componentInputs()[key]);
            });
        }
    }

    ngOnDestroy(): void {
        this.componentRef?.destroy();
    }
}
