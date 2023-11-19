import { Component, Input, OnInit } from '@angular/core';
import { DataService } from '../../services/data.service';
import { Weather } from '../../Weather';
@Component({
  selector: 'app-show-archieve',
  templateUrl: './show-archieve.component.html',
  styleUrls: ['./show-archieve.component.css']
})
export class ShowArchieveComponent implements OnInit {

  infWeather: Weather[];
  tableMode: boolean = true;
  countOfElements: number;
  countOfPages: number;
  countOfElementsOnPAge: number = 5;
  pageNumber: number = 1;

  isUpPage: boolean = true;
  isDownPage: boolean = true;

  months = [
    { id: 1, name: 'Январь' },
    { id: 2, name: 'Февраль' },
    { id: 3, name: 'Март' },
    { id: 4, name: 'Апрель' },
    { id: 5, name: 'Май' },
    { id: 6, name: 'Июнь' },
    { id: 7, name: 'Июль' },
    { id: 8, name: 'Август' },
    { id: 9, name: 'Сентябрь' },
    { id: 10, name: 'Октябрь' },
    { id: 11, name: 'Ноябрь' },
    { id: 12, name: 'Декбрь' }
  ];
  years = [
    2010, 2011, 2012, 2013
  ];
  public selectedMonth: number = 0;
  public selectedYear: number  = 0;
  constructor(private dataService: DataService) { }

  ngOnInit() {
  }

  loadWeather() {
    this.calcPages();
    this.dataService.getWeather(this.selectedMonth, this.selectedYear, this.pageNumber, this.countOfElementsOnPAge)
      .subscribe((data: any) => {
        this.infWeather = data;
      }
      );

    this.isUpPage = false;
    this.pageNumber = 1;

  }
  calcPages() {
    //получение количества записей
    this.dataService.getCountOfElementsToShow(this.selectedMonth, this.selectedYear)
      .subscribe((data: any) => {
        this.countOfElements = data;
        this.countOfPages = this.countOfElements / this.countOfElementsOnPAge;
        this.countOfPages = Math.floor(this.countOfPages);
        if (this.countOfPages * this.countOfElementsOnPAge < this.countOfElements) {
          this.countOfPages++;
        }
      }
      );
  }
  changePageUp() {
    debugger
    if (this.pageNumber === this.countOfPages - 1) {
      this.isUpPage = true;
      this.pageNumber++;
    }
    else if (this.pageNumber === this.countOfPages) {
      this.isDownPage = false;
      this.isUpPage = true;
    }
    else {
      this.pageNumber++;
      this.isDownPage = false;
    }
    this.dataService.getWeather(this.selectedMonth, this.selectedYear, this.pageNumber, this.countOfElementsOnPAge)
      .subscribe((data: any) => {
        this.infWeather = data;
      }
      );


  }

  changePageDown() {
    debugger
    if (this.pageNumber === 2) {
      this.isDownPage = true;
      this.pageNumber--;
    }
    else if (this.pageNumber === 1) {
      this.isDownPage = true;
      this.isUpPage = false;
    }
    else {
      this.pageNumber--;
      this.isUpPage = false;
    }
    this.dataService.getWeather(this.selectedMonth, this.selectedYear, this.pageNumber, this.countOfElementsOnPAge)
      .subscribe((data: any) => {
        this.infWeather = data;
      }
      );
  }
}
