import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/app/models/Evento';
import { EventoService } from 'src/app/services/evento.service';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {

  modalRef?: BsModalRef
  public eventos: Evento[] = []
  public eventoId = 0
  public eventosFiltrados: Evento[] = []

  widthImg: number = 125
  marginImg: number = 0
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

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.spinner.show();
    this.carregarEventos()
  }

  public carregarEventos(): void {

    const observer = {
      next: (eventos:Evento[]) => {
        this.eventos = eventos
        this.eventosFiltrados = this.eventos
      },
      error: (error: any) => {
        this.spinner.hide()
        this.toastr.error('Erro ao carregar os eventos', 'Erro !')
      },
      complete: () => this.spinner.hide()
    }

    this.eventoService.getEventos().subscribe(observer)
  }

  public switchImageState(): void {
    this.showImage = !this.showImage;
  }

  openModal(event: any, template: TemplateRef<any>, eventoId: number): void {
    event.stopPropagation();
    this.eventoId = eventoId
    this.modalRef = this.modalService.show(template, {class: 'modal-sm', backdrop: 'static'});
  }

  confirm(): void {
    this.modalRef?.hide()
    this.spinner.show()

    this.eventoService.deleteEvento(this.eventoId).subscribe(
      (result: any) => {
        if (result.message === 'Deletado') {
        this.toastr.success('Evento deletado com sucesso', 'Deletado !')
        this.carregarEventos()
      }
      },
      (error: any) => {
        console.error(error)
        this.toastr.error(`Erro ao tentar deletar o evento ${this.eventoId}`, 'Erro !')
      },
    ).add(() => this.spinner.hide())
  }
 
  decline(): void {
    this.modalRef?.hide();
  }

  detalheEvento(id: number): void {
    this.router.navigate([`eventos/detalhe/${id}`])
  }
}
