import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

  form!: FormGroup
  constructor(private fb: FormBuilder) { }

  get f(): any {
    return this.form.controls
  }

  ngOnInit(): void {
    this.validation()
  }

  private validation(): void {
    this.form = this.fb.group({
      primeiroNome: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(10)]],
      ultimoNome: ['', Validators.required, Validators.minLength(3), Validators.maxLength(10)],
      email: ['', Validators.required, Validators.email],
      userName: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(12)]],
      senha: ['', Validators.required, Validators.minLength(6), Validators.maxLength(15)],
      confirmaSenha: ['', [Validators.required]]
    })
  }
}
