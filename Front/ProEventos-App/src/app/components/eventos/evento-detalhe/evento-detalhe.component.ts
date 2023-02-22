import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

import { Evento } from 'src/app/models/Evento';
import { Lote } from 'src/app/models/Lote';
import { EventoService } from 'src/app/services/evento.service';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss'],
})
export class EventoDetalheComponent implements OnInit {

  evento = {} as Evento
  form!: FormGroup
  estadoSalvar = 'post'

  constructor(private fb: FormBuilder,
    private localeService: BsLocaleService,
    private activatedrouter: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
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
    const eventoIdParam = this.activatedrouter.snapshot.paramMap.get('id')

    if (eventoIdParam !== null) {
      this.spinner.show()

      this.estadoSalvar = 'put'

      const observer = {
        next: (evento: Evento) => {
          this.evento = { ... evento}
          this.form.patchValue(this.evento)
        },
        error: (error: any) => {
          this.spinner.hide()
          this.toastr.error('Erro ao carregar o evento', 'Erro !')
          console.error(error)
        },
        complete: () => this.spinner.hide()
      }

      this.eventoService.getEventoById(+eventoIdParam).subscribe(observer)
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

  public salvarAlteracao(): void {
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
}
