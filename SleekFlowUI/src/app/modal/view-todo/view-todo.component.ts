import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/common/api.service';
import { Status } from 'src/app/model/status';
import { Todo } from 'src/app/model/todo';
import { ViewTodoService } from './view-todo.service';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { Tag } from 'src/app/model/tag';
import { NgForm } from '@angular/forms';
import { SummaryService } from 'src/app/summary/summary.service';
import { AddTagService } from '../add-tag/add-tag.service';

@Component({
  selector: 'app-view-todo',
  templateUrl: './view-todo.component.html',
  styleUrls: ['./view-todo.component.scss']
})
export class ViewTodoComponent implements OnInit {

  public vTodo:Todo;
  public vtagList: Tag[] = [];
  public vselectedTags: Tag[] = [];
  public dropdownSettings: IDropdownSettings;

  constructor(private apiService: ApiService, private addTagService: AddTagService, private viewTodoService: ViewTodoService, private summaryService: SummaryService) {
    const today = new Date();
    this.vTodo = new Todo(
      0,'','','',today.toISOString().substring(0, 10),[]
    )

    this.dropdownSettings = {
      idField: 'tag_id',
      textField: 'tag_name',
      itemsShowLimit: 10,
      allowSearchFilter: true,
      enableCheckAll: false,
      defaultOpen: false
    };
  }

  ngOnInit(): void {
    this.init()
  }

  init() {
    this.viewTodoService.getResultObserver().subscribe((todo_id) => {
      if (Number(todo_id)){
        this.getTodo(Number(todo_id))
      }
    });

    this.addTagService.getRefreshTagObserver().subscribe((status) => {
      this.getTagList()
    });
  }
  getTodo(todo_id:number): void {
    this.apiService.getTodo(todo_id).subscribe((response: any) => {
      if (response) {
        this.vTodo = response;
        this.vselectedTags = this.vTodo.todo_tags
        const d = new Date(this.vTodo.todo_due_date);
        d.setHours(d.getHours()+8)
        this.vTodo.todo_due_date = d.toISOString().substring(0, 10)
        $('#viewTodoModal').modal('show'); 
      }
    })
  }

  editTodo(todo: Todo){
    
    this.apiService.UpdateTodo(todo)
      .subscribe(
        (response: any) => {
          this.closeModal()
          this.refreshPage()
        });
  }

  getTagList(): void {
    this.apiService.getTagList().subscribe((response: any) => {
      if (response) {
        this.vtagList = response;
      }
    })
  }

  addTagModal(){
    this.closeModal()
    $('#addTagModal').modal('show');
  }

  saveChanges(){
    console.log("save",this.vTodo.todo_name , this.vTodo.todo_due_date)
    if (this.vTodo.todo_name && this.vTodo.todo_due_date){
      this.vTodo.todo_tags = this.vselectedTags
      this.editTodo(this.vTodo)
    }
  }

  changeStatus(newStatus:string){
    this.vTodo.todo_status = newStatus
    this.editTodo(this.vTodo)
  }

  closeModal(){
    $('#viewTodoModal').modal('hide');
  }

  refreshPage() {
    this.summaryService.setRefresh()
  }

}
