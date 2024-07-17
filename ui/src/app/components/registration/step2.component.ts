import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CountryService as CountryApi, GetCountriesQueryResponse, GetProvinceQueryResponse } from '../../core/modules/openapi'
import { CommonModule } from '@angular/common';
import { UserService } from '../../store/user.service';

@Component({
    selector: 'registration-step2',
    templateUrl: './step2.component.html',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule]
})
export class RegistrationStep2Component implements OnInit
{
    step2Form: FormGroup;
    countries: GetCountriesQueryResponse[] = [];
    provinces: GetProvinceQueryResponse[] | undefined = [];

    constructor(private fb: FormBuilder, private api: CountryApi, private userService: UserService)
    {
        this.step2Form = new FormGroup({
            country: new FormControl('', [Validators.required]),
            province: new FormControl('', [Validators.required]),
        });
    }

    ngOnInit()
    {

        let thisSaver = this;
        this.api.countryGetCountries().subscribe({
            next(countries)
            {
                thisSaver.countries = countries
            }
        })

        let countrySelect = this.step2Form.get("country");
        if (countrySelect != null)
        {
            countrySelect.valueChanges.subscribe((countryId) =>
            {
                this.provinces = this.countries.find(x=>x.id === countryId)?.provinces;
            });
        }

    }

    onSave()
    {
        if (this.step2Form.valid)
        {
            if (this.step2Form.valid) {
                this.userService.updateStep2Data(this.step2Form.value);
                this.userService.submitRegistration();
              } else {
                this.step2Form.markAllAsTouched();
              }
        }
    }
}
