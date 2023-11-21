import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { DataService } from '../../services/data.service';
import { FileWithInfo } from '../../models/FileWithInfo';


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
  constructor(private dataService: DataService) {
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
    this.isSelectFileEnable = true;
    for (let i = 0; i < this.selectedFiles.length; i++) {
      this.dataService.saveData(this.selectedFiles[i])
        .subscribe((data: any) => {
          console.log(data)
          this.selectedFiles[i].info = data.Text;
          this.selectedFiles[i].isLoading = false;
          this.checkLoading();
        },
          (error) => {
            if (error.status === 504) {
              this.selectedFiles[i].info = `Status: ${error.status}, message: Превышен лимит ожидание сервера`;
            }
            else {
              debugger;
              this.selectedFiles[i].info = `Status: ${error.error.status}, message: ${error.error.detail}`;
            }
            
            this.selectedFiles[i].isLoading = false;
            this.checkLoading();
          });
    }
  }

  checkLoading() {
    for (let i = 0; i < this.selectedFiles.length; i++) {
      if (this.selectedFiles[i].isLoading === true) {
        return;
      }
      this.isSelectFileEnable = false;
      this.isUploadEnable = true;
    }
  }
}

