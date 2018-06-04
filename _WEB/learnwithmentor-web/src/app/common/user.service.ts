import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestMethod } from '@angular/http';
import { Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { catchError, tap } from 'rxjs/operators';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';
 
import {User} from'../common/models/user.model'

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  private userUrl = `${environment.apiUrl}/api/User`;

  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  getUsers(): Observable<User[]>{
    return this.http.get<User[]>(this.userUrl).pipe(
      catchError(this.handleError('getUsers', []))
    );
  }

  getUserById(id: number): Observable<User>{
    return this.http.get<User>(`${this.userUrl}/${id}`).pipe(
      catchError(this.handleError<User>(`getUser id=${id}`))
    );
  }

  deleteUser(user: User | number): Observable<User>{
    const id = typeof user === 'number' ? user : user.Id;
    return this.http.delete<User>(`${this.userUrl}/${id}`).pipe(
      catchError(this.handleError<User>(`getUser id=${id}`))
    );
  }

  createUser(user: User): Observable<User>{
    return this.http.post<User>(this.userUrl, user, this.httpOptions).pipe(
      catchError(this.handleError<User>('addUser'))
    );
  }

  editUser(user: User): Observable<User>{
    return this.http.put<User>(`${this.userUrl}/${user.Id}`, user, this.httpOptions).pipe(
      catchError(this.handleError<User>('editUser'))
    );
  }

  private handleError<T> (operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {   
      console.error(error);   
      return of(result as T);
    };
  }
}
