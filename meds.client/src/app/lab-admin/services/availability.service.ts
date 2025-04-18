import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class AvailabilityService {
  private apiUrl = "https://localhost:7217/TestTypes/lab-admin/availableTests";
  constructor(private http: HttpClient) { }

  getTestTypes(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}`, { withCredentials: true });
  }
  setTestTypes(availabilityList: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}`, availabilityList, { withCredentials: true });
  }
}
