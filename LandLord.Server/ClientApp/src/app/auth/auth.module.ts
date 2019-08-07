import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from './services/auth-service.service';
import { JwtInterceptor } from './interceptors/jwt-interceptor.service';
import { ErrorInterceptor } from './interceptors/error-interceptor';
import { LoginMenuComponent } from './components/login-menu/login-menu.component';
import { AuthGuard } from './guards/auth-guard';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    LoginMenuComponent,
  ],
  imports: [
    CommonModule,
    HttpClientModule,
  ],
  exports:[
    LoginMenuComponent,
  ],
})
export class AuthModule { }
