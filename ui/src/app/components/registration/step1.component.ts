import { CommonModule, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl, AbstractControl, ReactiveFormsModule  } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../store/user.service';

@Component({
    selector: 'registration-step1',
    templateUrl: './step1.component.html',
    imports: [CommonModule, ReactiveFormsModule ],
    standalone: true
})
export class RegistrationStep1Component
{
    step1Form: FormGroup;

    constructor(private fb: FormBuilder, private router: Router, private userService: UserService)
    {
        this.step1Form = new FormGroup({
            email: new FormControl('', [Validators.email, Validators.required]),
            password: new FormControl('', [
                Validators.required,
                Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]*$/)
            ]),
            confirmPassword: new FormControl('', [Validators.required]),
            agree: new FormControl(false, [Validators.requiredTrue])

        }, { validators: this.passwordMatchValidator });
    }

    passwordMatchValidator(group: AbstractControl): { [key: string]: any } | null
    {
        const password = group.get('password')?.value;
        const confirmPassword = group.get('confirmPassword')?.value;

        return password === confirmPassword ? null : { mismatch: true };
    }

    onNext(event: Event)
    {
        event.preventDefault();
        if (this.step1Form.valid)
        {
            this.userService.updateStep1Data(this.step1Form.value);
            this.router.navigate(['/step2']);
        }
        else
        {
            this.step1Form.markAllAsTouched();
        }
        
    }
}