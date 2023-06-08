import {AfterViewInit, Component, OnInit, ViewChild} from '@angular/core';
import { ApiService } from '../common/api.service';
import { Todo } from '../model/todo';
import { AddTodoService } from '../modal/add-todo/add-todo.service';
import { ViewTodoService } from '../modal/view-todo/view-todo.service';
import { SummaryService } from './summary.service';

@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.scss']
})
export class SummaryComponent implements OnInit {

  public todoList: Todo[] = [];
  public currentId = 0

  constructor(private apiService: ApiService,private addTodoService:AddTodoService,
    private viewTodoService:ViewTodoService, private summaryService: SummaryService) { }

  ngOnInit(): void {
    this.init()
  }

  init() {
    this.summaryService.getResultObserver().subscribe((refresh) => {
      console.log('inside',refresh)
      this.getTodoList()
    });
  }

  getTodoList(): void {
    
    this.apiService.getTodoList().subscribe((response: any) => {
      if (response) {
        this.todoList = response;
        console.log(this.todoList)
      }
    })
  }

  openAddTodoModal() {
    $('#addTodoModal').modal('show'); 
  }

  viewTodoModal(todo_id:number){
    this.viewTodoService.setTodoId(todo_id);
  }

  openDeleteTodoModal(todo_id:number) {
    this.currentId = todo_id
    $('#deleteTodoModal').modal('show'); 
  }

  closeModal(){
    $('#deleteTodoModal').modal('hide'); 
  }

  deleteTodo(){
    this.apiService.DeleteTodo(this.currentId)
      .subscribe(
        (response: any) => {
          this.closeModal()
          this.refreshPage()
        });
  }

  refreshPage() {
    this.getTodoList()
  }
  
}