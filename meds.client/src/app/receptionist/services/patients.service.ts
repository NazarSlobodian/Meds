import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PatientsService {

  private apiUrl = "https://localhost:7217/Patients";
  constructor(private http: HttpClient) { }
  getPatientsByName(fullName: string | null): Observable<any> {
    let params = new HttpParams();
    if (fullName) params = params.append('fullName', fullName);

    return this.http.get(this.apiUrl, { params, withCredentials: true });
  }
  getPatientsByNumber(phoneNumber: string | null): Observable<any> {
    let params = new HttpParams();
    if (phoneNumber) params = params.append('phoneNumber', phoneNumber);

    return this.http.get(this.apiUrl, { params, withCredentials: true });
  }
  addPatient(patient: any): Observable<any> {
    return this.http.post(this.apiUrl, patient, { withCredentials: true });
  }
}
