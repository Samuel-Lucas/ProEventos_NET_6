import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

import { Evento } from 'src/app/models/Evento';
import { Lote } from 'src/app/models/Lote';

import { EventoService } from 'src/app/services/evento.service';
import { LoteService } from 'src/app/services/lote.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss'],
})
export class EventoDetalheComponent implements OnInit {

  modalRef: BsModalRef
  eventoId: number
  evento = {} as Evento
  loteAtual = {id: 0, nome: '', indice: 0}
  form!: FormGroup
  estadoSalvar = 'post'
  imagemUrl = 'assets/upload-image.jpg'
  file: File

  constructor(private fb: FormBuilder,
    private localeService: BsLocaleService,
    private activatedrouter: ActivatedRoute,
    private eventoService: EventoService,
    private loteService: LoteService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private modalService: BsModalService,
    private router: Router)
  {
    this.localeService.use('pt-br')
  }

  get lotes(): FormArray {
    return this.form.get('lotes') as FormArray;
  }

  get f(): any {
    return this.form.controls
  }

  get modoEditar(): boolean {
    return this.estadoSalvar === 'put'
  }

  get bsConfig(): any {
    return {
      adaptativePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm a',
      containerClass:'theme-default',
      showWeekNumbers: false
    }
  }

  ngOnInit(): void {
    this.carregarEvento()
    this.validation()
  }

  public carregarEvento(): void {
    this.eventoId = +this.activatedrouter.snapshot.paramMap.get('id')

    if (this.eventoId !== null && this.eventoId !== 0) {
      this.spinner.show()

      this.estadoSalvar = 'put'

      const observer = {
        next: (evento: Evento) => {
          this.evento = { ... evento}
          this.form.patchValue(this.evento)

          if (this.evento.imagemUrl !==  '') {
            this.imagemUrl = `${environment.apiUrl}resources/images/${this.evento.imagemUrl}`
          }

          this.evento.lotes.forEach(lote => {
            this.lotes.push(this.criarLote(lote))
          })
        },
        error: (error: any) => {
          this.spinner.hide()
          this.toastr.error('Erro ao carregar o evento', 'Erro !')
          console.error(error)
        },
        complete: () => this.spinner.hide()
      }

      this.eventoService.getEventoById(this.eventoId).subscribe(observer)
    }
  }

  public validation(): void {
    this.form = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      imagemUrl: ['', Validators.required],
      lotes: this.fb.array([])
    })
  }

  public mudarValorData(value: Date, indice: number, campo: string): void {
    this.lotes.value[indice][campo] = value
  }

  public retornaTituloLote(nome: string): string {
    return nome === null || nome === ''? 'Nome do lote' : nome
  }

  public adicionarLote(): void {
    this.lotes.push(
      this.criarLote({id: 0} as Lote)
    )
  }

  criarLote(lote: Lote): FormGroup {
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      preco: [lote.preco, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      dataInicio: [lote.dataInicio, Validators.required],
      dataFim: [lote.dataFim, Validators.required]
    })
  }

  public resetForm(): void {
    this.form.reset()
  }

  public salvarEvento(): void {
    this.spinner.show()

    if (this.form.valid) {
      this.evento = (this.estadoSalvar === 'post')
                    ? {... this.form.value}
                    : {id: this.evento.id, ... this.form.value}

      this.eventoService[this.estadoSalvar](this.evento).subscribe(
        (eventoRetorno: Evento) => {
          this.toastr.success('Evento salvo com sucesso', 'Salvo !')
          this.router.navigate([`eventos/detalhe/${eventoRetorno.id}`])
        },
        (error: any) => {
          console.error(error)
          this.toastr.error('Erro ao tentar salvar o evento', 'Erro !')
        },
      ).add(() => this.spinner.hide())
    }
  }

  public salvarLotes(): void {
    this.spinner.show()

    if (this.form.controls['lotes'].valid) {
      this.loteService.saveLote(this.eventoId, this.form.value.lotes).subscribe(
        () => {
          this.toastr.success('Lotes salvos com sucesso', 'Salvo !')
          this.lotes.reset()
        },
        (error: any) => {
          console.error(error)
          this.toastr.error('Erro ao tentar salvar lotes', 'Erro !')
        },
      ).add(() => this.spinner.hide())
    }
  }

  public removerLote(template: TemplateRef<any>, indice: number): void {
    this.loteAtual.id = this.lotes.get(indice + '.id').value
    this.loteAtual.nome = this.lotes.get(indice + '.nome').value
    this.loteAtual.indice = indice

    this.modalRef = this.modalService.show(template, {class: 'modal-sm'})
  }

  confirmDeleteLote(): void {
    this.modalRef.hide()
    this.spinner.show()
    this.loteService.deleteLote(this.eventoId, this.loteAtual.id).subscribe(
      () => {
        this.toastr.success('Lote atual deletado com sucesso', 'Salvo !')
        this.lotes.removeAt(this.loteAtual.indice)
      },
      (error: any) => {
        console.error(error)
        this.toastr.error(`Erro ao tentar deletar lote: ${this.loteAtual.id}`, 'Erro !')
      },
    ).add(() => this.spinner.hide())
  }

  declineDeleteLote(): void {
    this.modalRef.hide()
  }

  onFileChange(ev: any): void {
    const reader = new FileReader()

    reader.onload = (event: any) => this.imagemUrl = event.target.result

    this.file = ev.target.files
    reader.readAsDataURL(this.file[0])

    this.uploadImagem()
  }

  uploadImagem(): void {
    this.spinner.show()
    this.eventoService.postUpload(this.eventoId, this.file).subscribe(
      () => {
        this.carregarEvento()
        this.toastr.success('Imagem atualiza com sucesso', 'Sucesso !')
      },
      (error: any) => {
        this.toastr.success('Erro ao fazer upload da imagem', 'Erro !')
        console.log(error)
      }
    ).add(() => this.spinner.hide())
  }
}
