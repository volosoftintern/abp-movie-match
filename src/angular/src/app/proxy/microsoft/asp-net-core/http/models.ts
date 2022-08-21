
export interface IFormFile {
  contentType?: string;
  contentDisposition?: string;
  headers: Record<string, StringValues>;
  length: number;
  name?: string;
  fileName?: string;
}
