import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BatchesService {

  private apiUrl = "https://localhost:7217/Patients/batches";
  constructor(private http: HttpClient) { }

  getBatches(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}`, {withCredentials: true});
  }
  getBatchResults(batchId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${batchId}`, { withCredentials: true });
  }
  getBatchResultsPdf(batchId: number): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/${batchId}/pdf`, { responseType: 'blob', withCredentials: true });
  }
}
