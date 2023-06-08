import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SummaryService {

  private Refresh$ = new BehaviorSubject<Object>({});

  constructor() { }

  getResultObserver(): Observable<Object> {
    return this.Refresh$.asObservable();
  }

  setRefresh(){
    console.log('refresh')
    this.Refresh$.next(!Boolean(this.Refresh$));
  }
}
