import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EventoService {
  baseurl = 'https://localhost:7194/api/eventos';
  constructor(private http: HttpClient) { }

  getEvento() {
    return this.http.get(this.baseurl)
}

}
