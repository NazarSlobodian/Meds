import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminTestService {
  private apiUrl = "https://localhost:7217/TestTypes/admin";
  constructor(private http: HttpClient) { }
  getAvailableTestTypes(page: number, pageSize: number): Observable<any> {
    let params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);
    return this.http.get(this.apiUrl, { params, withCredentials: true });
  }
  getAvailableTestPanels(page: number, pageSize: number): Observable<any> {
    let params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);
    return this.http.get(`${this.apiUrl}/panels`, { params, withCredentials: true });
  }
  getNormalValues(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/normals/${id}`, { withCredentials: true });
  }
  updateTestInfo(testType: any): Observable<any> {
    return this.http.put(`${this.apiUrl}`, testType, { withCredentials: true });
  }
  updatePanelInfo(testPanel: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/panels`, testPanel, { withCredentials: true });
  }
  addTestType(testType: any): Observable<any> {
    return this.http.post(`${this.apiUrl}`, testType, { withCredentials: true });
  }
  addTestPanel(testPanel: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/panels`, testPanel, { withCredentials: true });
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
