import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { UserService } from '../store/user.service';
import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate
{
    constructor(private userService: UserService, private router: Router) { }

    canActivate()
    {
        return this.userService.user$.pipe(
            map(user =>
            {

                if (user)
                {
                    return true;
                } else
                {
                    this.router.navigate(['step1']);
                    return false;
                }
            })
        );
    }
}