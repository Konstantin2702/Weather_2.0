import { Component } from '@angular/core';
import { DataService } from '../../services/data.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  providers: [DataService]
})
export class HomeComponent {
  tableMode: boolean = true;
  constructor(private dataService: DataService) { }
  ngOnInit() {

  }
}
