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
          localStorage.setItem('role', response.role); 
        }
      })
    );
  }
  getRole() {
    return localStorage.getItem("role");
    return this.role;
  }
  initRegistration(value: string): Observable<any> {
    const email = { value };
    return this.http.post<any>(`${this.apiUrl}/register`, email, { withCredentials: true });
  }
  submitCode(email: string, code: string): Observable<any> {
    const codeAndLogin = {login: email, password: code };
    return this.http.post<any>(`${this.apiUrl}/submitCode`, codeAndLogin, { withCredentials: true });
  }
  submitPassword(email: string, code: string): Observable<any> {
    const loginInfo = { login: email, password: code };
    return this.http.post<any>(`${this.apiUrl}/finishRegistration`,loginInfo, { withCredentials: true });
  }
}
