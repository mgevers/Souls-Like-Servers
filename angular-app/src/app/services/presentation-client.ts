import { HttpClient, HttpErrorResponse, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, Observable } from "rxjs";

import { environment } from "../../environments/environment";
import { AddMonsterRequest, Monster, UpdateMonsterRequest } from "./contracts";

@Injectable({
  providedIn: 'root',
})
export class PresentationClient {
  constructor(private httpClient: HttpClient)
  {
  }

  getMonsters(): Observable<Monster[]>  {
    const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');

    return this.httpClient.get<Monster[]>(`${environment.presentationUrl}/monsters`, {
      headers
    });
  }

  addMonster(request: AddMonsterRequest): Observable<Monster> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
    const json = JSON.stringify(request);

    console.log('request', request);
    console.log('json', json);

    return this.httpClient.post<Monster>(
      `${environment.presentationUrl}/monsters`,
      json, {
        headers
      });
  }

  updateMonster(request: UpdateMonsterRequest): Observable<Monster> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');

    return this.httpClient.put<Monster>(
      `${environment.presentationUrl}/monsters`,
      JSON.stringify(request), {
        headers
      });
  }

  removeMonster(id: string): Observable<string> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');

    const url = new URL(`${environment.presentationUrl}/monsters`);
    url.searchParams.set("monsterId", id);

    return this.httpClient.delete(url.toString(), {
        headers
      }).pipe(
        map(() => id),
      );
  }
}
