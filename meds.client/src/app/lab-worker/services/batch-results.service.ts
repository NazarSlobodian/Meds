import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BatchResultsService {

  private apiUrl = "https://localhost:7217/Patients/LabWorker/batches";
  private apiUrl2 = "https://localhost:7217/Patients/batches/results";
  constructor(private http: HttpClient) { }

  getResults(batchId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${batchId}`, { withCredentials: true });
  }
  submitResults(testOrders: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl2}`, testOrders, { withCredentials: true });
  }
}
