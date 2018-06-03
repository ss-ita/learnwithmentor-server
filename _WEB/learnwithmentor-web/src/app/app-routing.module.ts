import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserBlockComponent } from './user-block/user-block.component';

const routes: Routes = [
  { path: '', redirectTo: '/users', pathMatch: 'full' },
  //{ path: 'users/:id', component: UserBlockComponent }, TODO component for one user
  { path: 'users', component: UserBlockComponent }
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports:[RouterModule],
  declarations: []
})
export class AppRoutingModule { }
