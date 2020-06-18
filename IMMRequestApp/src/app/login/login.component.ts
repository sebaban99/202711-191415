import { Component, OnInit } from '@angular/core';
import {SessionService} from '../session.service';
import { Router } from '@angular/router';
import { LoginDTO } from 'src/app/Models/LoginDTO';
import { FormsModule }   from '@angular/forms';
import {OptionsMenuComponent} from '../options-menu/options-menu.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  email: string;
  password: string;

  constructor(private sessionService: SessionService, private router: Router) { }

  ngOnInit(): void {
  }

  onSubmit() {
    
    let loginDTO = new LoginDTO();
    loginDTO.Password = this.password;
    loginDTO.Email = this.email;

    this.sessionService.Login(loginDTO).subscribe(res => {
    this.router.navigate(["/home"]);
    },error => {alert(error.error)});

  }
}
