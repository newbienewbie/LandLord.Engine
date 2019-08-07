import { Injectable } from '@angular/core';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot, CanActivate } from '@angular/router';
import { AuthService } from '../services/auth-service.service';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private authenticationService: AuthService
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        let isAuthenticated = this.authenticationService.isAuthenticated();
        console.log(`xxxxxxxxxxxxx`,isAuthenticated);
        if (isAuthenticated) {
            return true;
        }
        return this.authenticationService.login()
            .pipe(map(o =>{
                console.log(`sssssssssssssssssssssssssssss`,o);
                if(this.authenticationService.isAuthenticated()){
                    return false;
                }
                console.log(`not authenticated, now redirect to login page`);
                this.router.navigate(['/identity/account/login'], { queryParams: { returnUrl: state.url } });
                return false;
            }));
    }
}