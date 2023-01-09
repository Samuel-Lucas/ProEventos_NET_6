import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidatorFields } from 'src/app/helpers/ValidatorField';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.css']
})
export class PerfilComponent implements OnInit {

  form!: FormGroup
  constructor(private fb: FormBuilder) { }

  get f(): any {
    return this.form.controls
  }

  ngOnInit() {
    this.validation()
  }

  private validation(): void {

    const formOptions: AbstractControlOptions = {
      validators: ValidatorFields.mustMatch('senha', 'confirmaSenha')
    }

    this.form = this.fb.group({
      titulo: ['', Validators.required],
      primeiroNome: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(10)]],
      ultimoNome: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(10)]],
      email: ['', [Validators.required, Validators.email]],
      telefone: ['', [Validators.required, Validators.minLength(9), Validators.maxLength(9)]],
      funcao: ['', Validators.required],
      descricao: ['', [Validators.required, Validators.minLength(20), Validators.maxLength(200)]],
      senha: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(15)]],
      confirmaSenha: ['', Validators.required]
    }, formOptions)
  }
}
