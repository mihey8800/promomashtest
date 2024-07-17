// user-page.component.ts
import { Component } from '@angular/core';
import { UserService, User } from '../../store/user.service';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
    selector: 'app-user-page',
    templateUrl: './user.component.html',
    imports: [CommonModule, ReactiveFormsModule],
    standalone: true
})
export class UserPageComponent
{
    user$: Observable<User | null>;

    constructor(private userService: UserService)
    {
        this.user$ = this.userService.user$;
    }

    logout()
    {
        this.userService.logout();
    }
}
