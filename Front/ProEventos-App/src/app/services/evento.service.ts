import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Evento } from '../models/Evento';

@Injectable()

export class EventoService {
  baseURL = `${environment.apiUrl}api/eventos`
  tokenHeader = new HttpHeaders({ 'Authorization': `Bearer ${JSON.parse(localStorage.getItem('user')).token}` });

  constructor(private http: HttpClient) { }

  public getEventos(): Observable<Evento[]> {
    return this.http
      .get<Evento[]>(this.baseURL, { headers: this.tokenHeader })
      .pipe(take(1))
  }

  public getEventoById(id: number): Observable<Evento> {
    return this.http
      .get<Evento>(`${this.baseURL}/BuscaEventoPorId/${id}`)
      .pipe(take(1))
  }

  public getEventosByTema(tema: string): Observable<Evento[]> {
    return this.http
      .get<Evento[]>(`${this.baseURL}/BuscaEventoPorTema/${tema}`)
      .pipe(take(1))
  }

  public post(evento: Evento): Observable<Evento> {
    return this.http
      .post<Evento>(this.baseURL, evento)
      .pipe(take(1))
  }

  public put(evento: Evento): Observable<Evento> {
    return this.http
      .put<Evento>(`${this.baseURL}/${evento.id}`, evento)
      .pipe(take(1))
  }

  public deleteEvento(id: number): Observable<any> {
    return this.http
      .delete(`${this.baseURL}/${id}`)
      .pipe(take(1))
  }

  public postUpload(eventoId: number, file: File): Observable<Evento> {
    const fileToUpload = file[0] as File
    const formData = new FormData()
    formData.append('file', fileToUpload)

    return this.http
      .post<Evento>(`${this.baseURL}/upload-image/${eventoId}`, formData)
      .pipe(take(1))
  }
}
