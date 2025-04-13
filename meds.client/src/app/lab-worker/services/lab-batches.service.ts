import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LabBatchesService {

  private apiUrl = "https://localhost:7217/Patients/LabWorker/batches";
  constructor(private http: HttpClient) { }

  getBatches(page: number, pageSize: number): Observable<any> {
    let params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);
    return this.http.get<any>(`${this.apiUrl}`, { params, withCredentials: true });
  }
}
