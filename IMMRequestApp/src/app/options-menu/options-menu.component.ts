import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-options-menu',
  templateUrl: './options-menu.component.html',
  styleUrls: ['./options-menu.component.css']
})
export class OptionsMenuComponent implements OnInit {

  constructor(private router: Router) { }

  userIsLogged: boolean;

  ngOnInit(): void {  
    this.checkUserLogged();
  }

  processLogout() {
    sessionStorage.clear();
    this.userIsLogged = false;
    this.router.navigate(["/home"]);
  }

  processLogin() {
    this.router.navigate(["/login"]);
  }

  checkUserLogged(){
    this.userIsLogged = false;
    let tokenInStorage = sessionStorage.getItem("currentUser");
    if(tokenInStorage != null){
      this.userIsLogged = true;
    }
  }
}
