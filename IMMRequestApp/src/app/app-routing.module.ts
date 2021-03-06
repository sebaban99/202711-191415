import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import {LoginComponent} from './login/login.component';
import {IsNotLoggedGuard} from './Guards/is-not-logged.guard';
import {IsLoggedGuard} from './Guards/is-logged.guard';
import {CreateRequestComponent} from './create-request/create-request.component';
import {CreateAdminComponent} from './create-admin/create-admin.component';
import {DeleteAdminComponent} from './delete-admin/delete-admin.component';
import { UpdateAdminComponent } from './update-admin/update-admin.component';
import { DeleteTypeComponent } from './delete-type/delete-type.component';
import { ImportationComponent } from './importation/importation.component';
import { CreateTypeComponent } from './create-type/create-type.component';
import { ReportComponent } from './report/report.component';
import { CheckRequestComponent } from './check-request/check-request.component';
import { UpdateRequestComponent } from './update-request/update-request.component';

const routes: Routes = [
  {path: '', redirectTo: 'home', pathMatch: 'full'},
  {path: 'home', component: HomeComponent},
  {path: 'login', component: LoginComponent, canActivate: [IsNotLoggedGuard]},
  {path: 'new-request', component: CreateRequestComponent},
  {path: 'new-admin', component: CreateAdminComponent, canActivate: [IsLoggedGuard]}, 
  {path: 'delete-admin', component: DeleteAdminComponent, canActivate: [IsLoggedGuard]}, //canActivate: [IsLoggedGuard]},
  {path: 'update-admin', component: UpdateAdminComponent, canActivate: [IsLoggedGuard]}, //canActivate: [IsLoggedGuard]},
  {path: 'delete-type', component: DeleteTypeComponent, canActivate: [IsLoggedGuard]}, //canActivate: [IsLoggedGuard]},
  {path: 'create-type', component: CreateTypeComponent, canActivate: [IsLoggedGuard]}, //canActivate: [IsLoggedGuard]},
  {path: 'report', component: ReportComponent, canActivate: [IsLoggedGuard]}, //canActivate: [IsLoggedGuard]},
  {path: 'check-request', component: CheckRequestComponent},
  {path: 'update-request', component: UpdateRequestComponent, canActivate: [IsLoggedGuard]}, //canActivate: [IsLoggedGuard]},
  {path: 'importation', component: ImportationComponent},
  {path: '**', redirectTo: 'home', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
