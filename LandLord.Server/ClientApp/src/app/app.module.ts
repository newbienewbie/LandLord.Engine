import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { RoomDetailComponent } from './room-detail/room-detail.component';
import { SignalrService } from './services/signalr.service';
import { CardConverterService } from './services/card-converter.service';
import { AuthModule } from './auth/auth.module';
import { JwtInterceptor } from './auth/interceptors/jwt-interceptor.service';
import { ErrorInterceptor } from './auth/interceptors/error-interceptor';
import { AuthGuard } from './auth/guards/auth-guard';
import { LoginMenuComponent } from './auth/components/login-menu/login-menu.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    RoomDetailComponent,
  ],
  imports: [
    AuthModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full',canActivate: [AuthGuard]  },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthGuard]},
      { path: 'room/:id', component: RoomDetailComponent, canActivate: [AuthGuard] }
    ])
  ],
  providers: [
    SignalrService, 
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    CardConverterService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
