import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpRequest } from '@angular/common/http';
import { FileWithInfo } from '../models/FileWithInfo';

@Injectable({ providedIn: 'root' })
export class DataService {
  constructor(private http: HttpClient) {
  }

  getWeather(month: number, year: number, pageNumber: number, countOFElementsOnPage: number) {
    if (month === null) month = 0;
    if (year === null) year = 0;
    let tempUrl = "api/getWeather"
    return this.http.get(tempUrl + "?" + "month=" + month + "&year=" + year + "&pageNumber=" +
      pageNumber + "&countOFElementsOnPage=" + countOFElementsOnPage);
  }

  getCountOfElementsToShow(month: number, year: number) {
    if (month === null) month = 0;
    if (year === null) year = 0;
    let tempUrl = "api/count";
    return this.http.get(tempUrl + "?" + "month=" + month + "&year=" + year);
  }

  saveData(file: FileWithInfo) {
    let fd = new FormData();
    file.isLoading = true;
    file.info = "";
    fd.append('files', file.file, file.file.name);
    return this.http.post('api/send', fd);
  }
}
