import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import {LoginComponent} from './login/login.component';
import {IsNotLoggedGuard} from './Guards/is-not-logged.guard';
import {IsLoggedGuard} from './Guards/is-logged.guard';
import {CreateRequestComponent} from './create-request/create-request.component';

const routes: Routes = [
  {path: '', redirectTo: 'home', pathMatch: 'full'},
  {path: 'home', component: HomeComponent},
  {path: 'login', component: LoginComponent, canActivate: [IsNotLoggedGuard]},
  {path: 'new-request', component: CreateRequestComponent},
  {path: '**', redirectTo: 'home', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
