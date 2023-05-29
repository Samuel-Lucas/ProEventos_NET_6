import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../models/identity/user';
import { Observable, map, take } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class AccountService {

  baseUrl = environment.apiUrl + 'api/account/'
  constructor(private http: HttpClient) { }

  public login(model: any): Observable<void> {
    return this.http.post<User>(this.baseUrl + 'login', model).pipe(
      take(1),
      map((response: User) => {
        const user = response;
        if (user) {

        }
      })
    );
  }
}
