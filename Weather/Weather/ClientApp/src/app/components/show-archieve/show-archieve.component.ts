import { Component, Input, OnInit } from '@angular/core';
import { DataService } from '../../services/data.service';
import { Weather } from '../../models/Weather';
@Component({
  selector: 'app-show-archieve',
  templateUrl: './show-archieve.component.html',
  styleUrls: ['./show-archieve.component.css']
})
export class ShowArchieveComponent implements OnInit {

  infWeather: Weather[];
  isLoading = false;
  tableMode = true;
  countOfElements: number;
  countOfPages: number;
  countOfElementsOnPAge = 5;
  pageNumber = 1;
  errorMessage: string = "";

  isUpPage = false;
  isDownPage = false;

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
  years: number[] = [];
  public selectedMonth = 0;
  public selectedYear = 0;
  constructor(private dataService: DataService) {
    var currentYear = new Date().getFullYear();
    for (let i = 1970; i <= currentYear; i++) {
      this.years.push(i);
    }
  }

  ngOnInit() {
  }

  loadWeather() {
    this.errorMessage = "";
    this.infWeather = [];
    this.isLoading = true;
    this.pageNumber = 1;
    this.dataService.getCountOfElementsToShow(this.selectedMonth, this.selectedYear)
      .subscribe((data: any) => {
        this.countOfElements = data;
        this.countOfPages = ~~(this.countOfElements / this.countOfElementsOnPAge) + 1;
        if (this.countOfPages * this.countOfElementsOnPAge < this.countOfElements) {
          this.countOfPages++;
        }
        this.isUpPage = this.pageNumber < this.countOfPages;

        this.dataService.getWeather(this.selectedMonth, this.selectedYear, this.pageNumber, this.countOfElementsOnPAge)
          .subscribe((data: any) => {
            this.infWeather = data;
            this.isLoading = false;
          },
            (error) => {
              debugger;
              this.errorMessage = `Status: ${error.error.status}, message: ${error.error.detail}`;
              this.countOfPages = 1;
              this.pageNumber = 1;
              this.isLoading = false;
            });
      },
        (error) => {
          debugger;
          this.isLoading = false;
          this.errorMessage = `Status: ${error.error.status}, message: ${error.error.detail}`;
        });
  }
  calcPages() {
    this.dataService.getCountOfElementsToShow(this.selectedMonth, this.selectedYear)
      .subscribe((data: any) => {
        this.countOfElements = data;
        this.countOfPages = ~~(this.countOfElements / this.countOfElementsOnPAge) + 1;
        if (this.countOfPages * this.countOfElementsOnPAge < this.countOfElements) {
          this.countOfPages++;
        }
        this.isUpPage = this.pageNumber < this.countOfPages;
      },
        (error) => {
          this.errorMessage = `Status: ${error.error.status}, message: ${error.error.detail}`;
        });
  }
  changePageUp() {
    this.infWeather = [];
    this.isLoading = true;
    this.pageNumber++;
    this.isUpPage = this.pageNumber < this.countOfPages;
    this.isDownPage = this.pageNumber > 1;

    this.dataService.getWeather(this.selectedMonth, this.selectedYear, this.pageNumber, this.countOfElementsOnPAge)
      .subscribe((data: any) => {
        this.infWeather = data;
        this.isLoading = false;
      },
        (error) => {
          this.errorMessage = `Status: ${error.error.status}, message: ${error.error.detail}`;
        });


  }

  changePageDown() {
    this.infWeather = [];
    this.isLoading = true;
    this.pageNumber--;
    this.isUpPage = this.pageNumber < this.countOfPages;
    this.isDownPage = this.pageNumber > 1;

    this.dataService.getWeather(this.selectedMonth, this.selectedYear, this.pageNumber, this.countOfElementsOnPAge)
      .subscribe((data: any) => {
        this.infWeather = data;
        this.isLoading = false;
      },
        (error) => {
          this.errorMessage = `Status: ${error.error.status}, message: ${error.error.detail}`;
        });
  }
}
