import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { DataService } from '../../services/data.service';


@Component({
  selector: 'app-download',
  templateUrl: './download-archieve.component.html',
  providers: [DataService]
})
export class DownloadArchieveComponent {
  tableMode: boolean = true;
  selectedFile: File[] = new Array;
  status: string;
  isLoading: boolean = false;
  constructor(private dataService: DataService, private http: HttpClient) { }
  ngOnInit() {
  }

  onFileSelected(event: Event) {
    let files = (event.target as HTMLInputElement).files;
    if (files !== null) {
      let i = 0;
      while (i < files.length) {
        let currentFile = files.item(i);
        i++;
        if (currentFile !== null) {
          this.selectedFile.push(currentFile);
        }
      }
    }

  }

  onUpload() {
    debugger;
    this.status = "";
    this.isLoading = true;
    const fd = new FormData();

    for (let i = 0; i < this.selectedFile.length; i++) {
      fd.append('files', this.selectedFile[i], this.selectedFile[i].name);
    }
    this.http.post('api/send', fd)
      .subscribe((data: any) => {
        this.status = data.Text;
        this.isLoading = false;
      });
  }
}

