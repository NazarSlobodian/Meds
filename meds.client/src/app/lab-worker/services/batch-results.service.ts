import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BatchResultsService {

  private apiUrl = "https://localhost:7217/Patients/LabWorker/batches";
  constructor(private http: HttpClient) { }

  getResults(batchId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${batchId}`, { withCredentials: true });
  }
}
