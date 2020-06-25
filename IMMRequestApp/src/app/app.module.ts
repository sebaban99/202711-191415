import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule }   from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { OptionsMenuComponent } from './options-menu/options-menu.component';
import { LoginComponent } from './login/login.component';
import { CreateRequestComponent } from './create-request/create-request.component';
import { CreateAdminComponent } from './create-admin/create-admin.component';
import { DeleteAdminComponent } from './delete-admin/delete-admin.component';
import { UpdateAdminComponent } from './update-admin/update-admin.component';
import { DeleteTypeComponent } from './delete-type/delete-type.component';
import { FooterComponent } from './footer/footer.component';
import { ImportationComponent } from './importation/importation.component';
import { CreateTypeComponent } from './create-type/create-type.component';
import { ReportComponent } from './report/report.component';
import { CheckRequestComponent } from './check-request/check-request.component';
import { UpdateRequestComponent } from './update-request/update-request.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    OptionsMenuComponent,
    LoginComponent,
    CreateRequestComponent,
    CreateAdminComponent,
    DeleteAdminComponent,
    UpdateAdminComponent,
    DeleteTypeComponent,
    FooterComponent,
    ImportationComponent,
    CreateTypeComponent,
    ReportComponent,
    CheckRequestComponent,
    UpdateRequestComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
  ],
  providers: [HttpClientModule],
  bootstrap: [AppComponent]
})
export class AppModule { }
