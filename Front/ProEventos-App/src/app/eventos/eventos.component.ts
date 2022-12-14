import { Component, OnInit } from '@angular/core';
import { Evento } from '../models/Evento';
import { EventoService } from '../services/evento.service';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss'],
  // providers: [EventoService]
})
export class EventosComponent implements OnInit {

  public eventos: Evento[] = []
  public eventosFiltrados: Evento[] = []

  widthImg: number = 125
  marginImg: number = 2
  showImage: boolean = true
  private _filtroLista: string = ''

  public get filtroLista(): string {
    return this._filtroLista
  }

  public set filtroLista(value: string) {
    this._filtroLista = value
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos
  }

  filtrarEventos(filtrarPor: string): Evento[] {
    filtrarPor = filtrarPor.toLocaleLowerCase()
    return this.eventos.filter(
      (evento: any) => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
      evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    )
  }

  constructor(private eventoService: EventoService) { }

  ngOnInit(): void {
    this.getEventos()
  }

  public getEventos(): void {

    const observer = {
      next: (eventos:Evento[]) => {
        this.eventos = eventos
        this.eventosFiltrados = this.eventos
      },
      error: (error: any) => console.log(error),
      complete: () => {}
    }

    this.eventoService.getEventos().subscribe(observer);
  }

  public switchImageState(): void {
    this.showImage = !this.showImage;
  }
}
