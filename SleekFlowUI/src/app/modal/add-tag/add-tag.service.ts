import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AddTagService {

  private Refresh$ = new BehaviorSubject<Object>({});

  constructor() { }

  getRefreshTagObserver(): Observable<Object> {
    return this.Refresh$.asObservable();
  }

  setRefresh(){
    this.Refresh$.next(!Boolean(this.Refresh$));
  }
}
