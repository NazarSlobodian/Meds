import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PatientsService {

  private apiUrl = "https://localhost:7217/Patients";
  constructor(private http: HttpClient) { }
  getPatients(fullName: string | null, phone: string | null, email: string | null, dateOfBirth: string | null, page: number, pageSize: number): Observable<any> {
    let params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);
    if (fullName) params = params.append('fullName', fullName.trim());
    if (phone) params = params.append('phone', phone.trim());
    if (email) params = params.append('email', email.trim());
    if (dateOfBirth) params = params.append('dateOfBirth', dateOfBirth.trim());

    return this.http.get(this.apiUrl, { params, withCredentials: true });
  }
  addPatient(patient: any): Observable<any> {
    return this.http.post(this.apiUrl, patient, { withCredentials: true });
  }
  deletePatient(patientId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${patientId}`, { withCredentials: true });
  }
}
