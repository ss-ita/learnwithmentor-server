import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestMethod } from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';
 
import {User} from'../common/models/user.model'

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor() { }
}
