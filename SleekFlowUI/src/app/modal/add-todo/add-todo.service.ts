import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AddTodoService {

  private Result$ = new BehaviorSubject<Object>({});

  constructor() { }

  getResultObserver(): Observable<Object> {
    return this.Result$.asObservable();
  }

  setTodoId(todo_id:number){
    this.Result$.next(todo_id);
  }
}
