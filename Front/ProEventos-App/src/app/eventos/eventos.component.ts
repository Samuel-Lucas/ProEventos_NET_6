import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  public eventos:any = [
    {
      Tema: 'Angular',
      Local: 'Belo Horizonte'
    },
    {
      Tema: '.NET 6',
      Local: 'São Paulo'
    },
    {
      Tema: 'Java',
      Local: 'Recife'
    }
  ]

  constructor() { }

  ngOnInit(): void {
  }

}
