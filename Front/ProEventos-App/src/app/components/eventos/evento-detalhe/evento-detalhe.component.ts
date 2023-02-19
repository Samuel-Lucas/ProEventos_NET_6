import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

import { Evento } from 'src/app/models/Evento';
import { EventoService } from 'src/app/services/evento.service';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss'],
})
export class EventoDetalheComponent implements OnInit {

  evento = {} as Evento
  form!: FormGroup
  constructor(private fb: FormBuilder,
    private localeService: BsLocaleService,
    private router: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService)
  {
    this.localeService.use('pt-br')
  }

  get f(): any {
    return this.form.controls
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
    const eventoIdParam = this.router.snapshot.paramMap.get('id')

    if (eventoIdParam !== null) {
      this.spinner.show()

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
      imagemUrl: ['', Validators.required]
    })
  }

  public resetForm(): void {
    this.form.reset()
  }
}
