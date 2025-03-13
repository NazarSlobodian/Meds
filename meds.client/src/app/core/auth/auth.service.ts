import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = "https://localhost:7217/Login";
  private role = null;
  constructor(private http: HttpClient) { }
  login(login: string, password: string): Observable<any> {
    const loginData = { login, password };
    return this.http.post<any>(`${this.apiUrl}`, loginData, { withCredentials: true }).pipe(
      tap(response => {
        if (response && response.role) {
          this.role = response.role;
        }
      })
    );
  }
  getRole() {
    return this.role;
  }
}
