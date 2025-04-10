import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TechTestService {
  private apiUrl1 = "https://localhost:7217/TestTypes/receptionist";
  private apiUrl2 = "https://localhost:7217/Patients/submit-batch";
  constructor(private http: HttpClient) { }
  getAvailableTestTypes(): Observable<any> {
    return this.http.get(this.apiUrl1, { withCredentials: true });
  }
  submitBatch(patientId: number, testTypesIds: any, panelsIds: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl2}/${patientId}`, { testTypesIds, panelsIds }, { withCredentials: true });
  }
}
