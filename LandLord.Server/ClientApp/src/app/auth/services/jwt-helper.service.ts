import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class JwtHelperService {

  constructor() { }

  private parseJwt(token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map(function (c) { return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2); })
        .join('')
    );
    return JSON.parse(jsonPayload);
  };

  isExpired(tokenStr) {
    var token = this.parseJwt(tokenStr);
    var exp = token.exp * 1000;
    const skew = 10 * 60 * 1000;
    var now = new Date().getTime();
    return exp + skew > now;
  }



}
