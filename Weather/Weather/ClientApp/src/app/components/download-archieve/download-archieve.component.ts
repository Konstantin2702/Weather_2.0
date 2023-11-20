import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { DataService } from '../../services/data.service';
import { FileWithInfo } from '../../models/FileWithInfo';
import { catchError } from 'rxjs';


@Component({
  selector: 'app-download',
  templateUrl: './download-archieve.component.html',
  providers: [DataService]
})

export class DownloadArchieveComponent {
  tableMode: boolean = true;
  selectedFiles: FileWithInfo[] = [];
  isUploadEnable = false;
  isSelectFileEnable = false;
  isRefreshFiles: boolean = true;
  constructor(private http: HttpClient) {
  }
  ngOnInit() {
  }

  onFileSelected(event: Event) {
    this.selectedFiles = [];
    let files = (event.target as HTMLInputElement).files;
    this.isUploadEnable = true;
    this.isRefreshFiles = true;
    if (files !== null) {
      let i = 0;
      while (i < files.length) {
        let currentFile = files.item(i);
        i++;
        if (currentFile !== null) {
          this.selectedFiles.push(new FileWithInfo(currentFile));
        }
      }
    }
  }

  onUpload() {
    if (!this.isRefreshFiles) {
      if (confirm("Вы уже загрузили эти файлы, продолжить?")) {
        this.load();
        this.isUploadEnable = false;
        this.isRefreshFiles = false;
      }
    }
    else {
      this.load();
      this.isUploadEnable = false;
      this.isRefreshFiles = false;
    }
    
  }

  load() {
    debugger;
    this.isSelectFileEnable = true;
    for (let i = 0; i < this.selectedFiles.length; i++) {
      let fd = new FormData();
      this.selectedFiles[i].isLoading = true;
      this.selectedFiles[i].info = "";
      fd.append('files', this.selectedFiles[i].file, this.selectedFiles[i].file.name);
      this.http.post('api/send', fd)
        .subscribe((data: any) => {
          console.log(data)
          this.selectedFiles[i].info = data.Text;
          this.selectedFiles[i].isLoading = false;
          this.checkLoading();
        },
          (error) => {
            debugger;
            this.selectedFiles[i].info = `Status: ${error.error.status}, message: ${error.error.detail}` ;
            console.log(error);
            this.selectedFiles[i].isLoading = false;
            this.checkLoading();
          });
    }
  }

  checkLoading() {
    debugger;
    for (let i = 0; i < this.selectedFiles.length; i++) {
      if (this.selectedFiles[i].isLoading === true) {
        return;
      }
      this.isSelectFileEnable = false;
      this.isUploadEnable = true;
    }
  }
}

