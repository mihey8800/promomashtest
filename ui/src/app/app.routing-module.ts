import { NgModule } from "@angular/core";
import { BASE_PATH } from "./core/modules/openapi/variables";
import { RegistrationStep1Component } from './components/registration/step1.component';
import { RegistrationStep2Component } from './components/registration/step2.component';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { UserPageComponent } from "./components/user/user.component";
const routes: Routes = [
    { path: '', redirectTo: '/user', pathMatch: 'full' },
    { path: 'step1', component: RegistrationStep1Component },
    { path: 'step2', component: RegistrationStep2Component },
    { path: 'user', component: UserPageComponent, canActivate: [AuthGuard] }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
    providers: [{ provide: BASE_PATH, useValue: '' }]
})
export class AppRoutingModule { }