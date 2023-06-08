import { HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';


export class AppConstants {
  public static readonly apiUrl: string = environment.apiUrl;
  public static readonly environment: string = environment.environmentName;
  public static readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json',
    })
  };
}