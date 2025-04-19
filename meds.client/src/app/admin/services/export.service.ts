import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ExportService {
  private apiUrl2 = "https://localhost:7217/Export/jsonTask";
  constructor(private http: HttpClient) { }
  getBatchesJson(): Observable<Blob> {
    return this.http.get(`${this.apiUrl2}`, { responseType: 'blob', withCredentials: true });
  }
}
