export class FileWithInfo {
  file: File;
  info: string;
  isLoading: boolean;
  constructor(file: File) {
    this.file = file;
    this.info = "";
    this.isLoading = false;
  }
}
