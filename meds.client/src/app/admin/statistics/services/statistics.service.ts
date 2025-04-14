import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {

  private apiUrl = "https://localhost:7217/Statistics";
  constructor(private http: HttpClient) { }
  getYearlyRevenue(): Observable<any> {
    return this.http.get(`${this.apiUrl}/yearlyRevenue`, { withCredentials: true });
  }
  getTestOrdersNumbers(): Observable<any> {
    return this.http.get(`${this.apiUrl}/testOrdersNumbers`, { withCredentials: true });
  }
  getClientDistribution(): Observable<any> {
    return this.http.get(`${this.apiUrl}/clientDistribution`, { withCredentials: true });
  }
}
