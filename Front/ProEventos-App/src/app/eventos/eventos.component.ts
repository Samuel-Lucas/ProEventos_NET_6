import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  public eventos:any = [];
  widthImg: number =125
  marginImg: number = 2
  showImage: boolean = true;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getEventos();
  }

  public getEventos(): any {

    this.http.get('https://localhost:7194/api/eventos').subscribe(
      response => this.eventos = response,
      error => console.log(error)
    );
  }

  public switchImageState() {
    this.showImage = !this.showImage;
  }
}
