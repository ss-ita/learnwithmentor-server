import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-user-block',
  templateUrl: './user-block.component.html',
  styleUrls: ['./user-block.component.css']
})

export class UserBlockComponent implements OnInit {
  Name = 'Olena';
  Surname = 'Vitiv';
  Email = 'olena@gmail.com';
  Role = 'mentor';
  
  constructor() { }

  ngOnInit() {
  }

}
