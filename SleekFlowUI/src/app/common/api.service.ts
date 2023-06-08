import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { AppConstants } from './common';
import { map, share, publishReplay, shareReplay } from 'rxjs/operators';
import { Todo } from '../model/todo';
import { Tag } from '../model/tag';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(public http: HttpClient) { }

  public getTodoList() {
    return this.http.get<any>(`${AppConstants.apiUrl}api/Todos`, AppConstants.httpOptions).pipe(
      map(response => {
        return  response
      },
        (error: any) => {
        })
    );
  }

  public getTodo(id:number) {
    return this.http.get<any>(`${AppConstants.apiUrl}api/Todos/${id}`, AppConstants.httpOptions).pipe(
      map(response => {
        if (response){
          return response[0]
        }else{
          return null
        }
      },
        (error: any) => {
        })
    );
  }

  public getTagList() {
    return this.http.get<any>(`${AppConstants.apiUrl}api/Tags`, AppConstants.httpOptions).pipe(
      map(response => {
        return  response
      },
        (error: any) => {
        })
    );
  }

  public getStatusList() {
    return this.http.get<any>(`${AppConstants.apiUrl}api/Status`, AppConstants.httpOptions).pipe(
      map(response => {
        return  response
      },
        (error: any) => {
        })
    );
  }

  AddTodo(todo: Todo): Observable<any> {
    return this.http.post<any>(`${AppConstants.apiUrl}api/Todos`, JSON.stringify(todo), AppConstants.httpOptions);
  }

  UpdateTodo(todo: Todo): Observable<any> {
    return this.http.put<any>(`${AppConstants.apiUrl}api/Todos`, JSON.stringify(todo), AppConstants.httpOptions);
  }

  DeleteTodo(todo_id: number): Observable<any> {
    return this.http.delete<any>(`${AppConstants.apiUrl}api/Todos/${todo_id}`, AppConstants.httpOptions);
  }

  AddTag(tag: Tag): Observable<any> {
    return this.http.post<any>(`${AppConstants.apiUrl}api/Tags`, JSON.stringify(tag), AppConstants.httpOptions);
  }
}
