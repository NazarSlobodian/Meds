import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LabBatchesService {

  private apiUrl = "https://localhost:7217/LabWorker/batches";
  constructor(private http: HttpClient) { }

  getBatches(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}`, { withCredentials: true });
  }
}
