import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { AuthService } from '../core/modules/openapi';
import { Router } from '@angular/router';
export interface User
{
    email: string;
    country: string;
    province: string;
}

interface RegistrationData
{
    step1: {
        email: string;
        password: string;
    };
    step2: {
        country: string;
        province: string;
    };
}

@Injectable({
    providedIn: 'root'
})
export class UserService
{
    private userSubject = new BehaviorSubject<User | null>(null);
    private tokenSubject = new BehaviorSubject<string | null>(null);

    user$ = this.userSubject.asObservable();
    token$ = this.tokenSubject.asObservable();


    constructor(private authService: AuthService, private router: Router) { }

    private registrationData = new BehaviorSubject<RegistrationData>({
        step1: {
            email: '',
            password: '',
        },
        step2: {
            country: '',
            province: '',
        }
    });

    getRegistrationData()
    {
        return this.registrationData.asObservable();
    }

    updateStep1Data(step1Data: RegistrationData['step1'])
    {
        const currentData = this.registrationData.value;
        this.registrationData.next({ ...currentData, step1: step1Data });
    }

    updateStep2Data(step2Data: RegistrationData['step2'])
    {
        const currentData = this.registrationData.value;
        this.registrationData.next({ ...currentData, step2: step2Data });
    }

    submitRegistration()
    {
        const data = this.registrationData.value;
        console.log(data);
        this.authService.authRegister({
            countryId: data.step2.country,
            provinceId: data.step2.province,
            login: data.step1.email,
            password: data.step1.password,
        }).subscribe({
            next: () =>
            {
                this.login(data.step1.email, data.step1.password);
            },
            error: (e) => console.error('Logout failed', e),
            complete: () => console.info('complete')
        });
    }

    login(username: string, password: string)
    {
        return this.authService.authLogin(username, password).subscribe({
            next: (response) =>
            {
                console.log("login performed", response)
                this.tokenSubject.next(response["access_token"]);
                const userInfo = this.decodeToken(response["access_token"]);
                console.log("token", userInfo)
                this.userSubject.next(userInfo);
                this.router.navigate(['user']);
            },
            error: (e) => console.error(e),
            complete: () => console.info('complete')
        });
    }

    logout()
    {
        return this.authService.authLogout({}).subscribe({
            next: () =>
            {
                this.tokenSubject.next(null);
                this.userSubject.next(null);
                this.router.navigate(['step1']);
            },
            error: (e) => console.error('Logout failed', e),
            complete: () => console.info('complete')
        });
    }

    private decodeToken(token: string): User
    {
        const decoded: any = jwtDecode(token);
        console.log(decoded);
        return {
            email: decoded.email,
            country: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/country'],
            province: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/stateorprovince']
        };
    }
}