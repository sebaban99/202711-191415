import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router} from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class IsNotLoggedGuard implements CanActivate {
  
  constructor(private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot): boolean {
    let isAdminLoggedIn = sessionStorage.getItem("currentUser");
    if (isAdminLoggedIn == null) {
      return true;
    } else {
        this.router.navigate(['/home']);
      return false;
    }
  }
  
}
