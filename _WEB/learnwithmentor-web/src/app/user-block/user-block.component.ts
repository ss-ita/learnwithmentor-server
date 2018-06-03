import { Component, OnInit } from '@angular/core';
import { User } from '../common/models/user.model';
import { UserService } from '../common/user.service';


@Component({
  selector: 'app-user-block',
  templateUrl: './user-block.component.html',
  styleUrls: ['./user-block.component.css']
})

export class UserBlockComponent implements OnInit {
  
  users: User[];
  
  constructor(private userService: UserService) { }

  getUsers(): void {
    this.userService.getUsers()
    .subscribe(users => this.users = users)
  }

  add(user: User): void{
    this.userService.createUser(user).subscribe(user => {
      this.users.push(user);
    });
  }

  delete(user: User): void {
    this.users = this.users.filter(h => h !== user);
    this.userService.deleteUser(user).subscribe();
  }

  ngOnInit() {
    this.getUsers();
  }

}
