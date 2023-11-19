import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { DataService } from '../app/services/data.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  providers: [DataService]
})
export class AppComponent implements OnInit {
  tableMode: boolean = true;          // табличный режим
  str: String = "";
  constructor(private dataService: DataService) { }
  ngOnInit() {

  }
}
