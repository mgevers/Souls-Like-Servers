import { OutputEmitterRef } from "@angular/core";

export interface AppForm<T> {
    submitted: OutputEmitterRef<T>;
}
