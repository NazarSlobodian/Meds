import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ActivityLogsService {
  private apiUrl = "https://localhost:7217/Statistics/activityLogs";
  constructor(private http: HttpClient) { }
  getActivityLogs(begin: string, end: string, page: number, pageSize: number): Observable<any> {
    return this.http.post(`${this.apiUrl}`, { begin: begin, end: end, page: page, pageSize: pageSize }, { withCredentials: true });
  }
}
