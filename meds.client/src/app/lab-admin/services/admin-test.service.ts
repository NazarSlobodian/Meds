import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminTestService {
  private apiUrl = "https://localhost:7217/TestTypes/admin";
  constructor(private http: HttpClient) { }
  getAvailableTestTypes(): Observable<any> {
    return this.http.get(this.apiUrl, { withCredentials: true });
  }
  getNormalValues(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/normals/${id}`, { withCredentials: true });
  }
  updateTestInfo(testType: any): Observable<any> {
    return this.http.put(`${this.apiUrl}`, testType, { withCredentials: true });
  }
  addTestType(testType: any): Observable<any> {
    return this.http.post(`${this.apiUrl}`, testType, { withCredentials: true });
  }
  deleteTestNormalValue(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/normals/${id}`, { withCredentials: true });
  }
  updateTestNormalValue(testNormalValue: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/normals`, testNormalValue, { withCredentials: true });
  }
  addTestNormalValue(testNormalValue: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/normals`, testNormalValue, { withCredentials: true });
  }
}
