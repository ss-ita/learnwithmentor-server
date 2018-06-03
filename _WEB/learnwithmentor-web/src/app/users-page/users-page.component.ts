import { Component, OnInit } from '@angular/core';
import {MatInputModule} from '@angular/material/input';
import {MatSelectModule} from '@angular/material/select';

@Component({
  selector: 'app-users-page',
  templateUrl: './users-page.component.html',
  styleUrls: ['./users-page.component.css']
})

export class UsersPageComponent implements OnInit {

  roles = [
    {value: 'admin', viewValue: 'Admin'},
    {value: 'mentor', viewValue: 'Mentor'},
    {value: 'student', viewValue: 'Student'}
  ]; 

  constructor() { }

  ngOnInit() {
  }

  

}
