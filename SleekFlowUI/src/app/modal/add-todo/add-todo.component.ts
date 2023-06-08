import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ApiService } from 'src/app/common/api.service';
import { Tag } from 'src/app/model/tag';
import { Todo } from 'src/app/model/todo';
import { AddTodoService } from './add-todo.service';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { SummaryService } from 'src/app/summary/summary.service';
import { AddTagService } from '../add-tag/add-tag.service';

@Component({
  selector: 'app-add-todo',
  templateUrl: './add-todo.component.html',
  styleUrls: ['./add-todo.component.scss']
})
export class AddTodoComponent implements OnInit {

  public todo_name:string = ''
  public todo_description:string = ''
  public todo_due_date:any
  public atodo:Todo;
  public atagList: Tag[] = [];
  public aselectedTags: Tag[] = [];
  public dropdownSettings: IDropdownSettings;

  constructor(private apiService: ApiService, private addTagService: AddTagService, private addTodoService: AddTodoService, private summaryService: SummaryService) { 
    const today = new Date();
    this.atodo = new Todo(
      0,'','','Not Started',today.toISOString().substring(0, 10),[]
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
    this.init();
  }

  init() {
    // this.addTodoService.getResultObserver().subscribe((status) => {
    //   console.log(status)
    // });

    this.addTagService.getRefreshTagObserver().subscribe((status) => {
      this.getTagList()
    });
  }

  getTodo(todo_id:number): void {
    this.apiService.getTodo(todo_id).subscribe((response: any) => {
      if (response) {
        this.atodo = response;
      }
    })
  }

  getTagList(): void {
    this.apiService.getTagList().subscribe((response: any) => {
      if (response) {
        this.atagList = response;
        console.log(this.atagList)
      }
    })
  }

  closeModal(){
    $('#addTodoModal').modal('hide');
  }
  
  addTodo(todo: Todo){
    
    this.apiService.AddTodo(todo)
      .subscribe(
        (response: any) => {
          this.closeModal()
          this.refreshPage()
        });
  }

  public onConfirm(f: NgForm) {
    console.log("Checking",f.valid,this.atodo)
    if (f.valid && this.atodo.todo_name && this.atodo.todo_due_date){
      this.atodo.todo_tags = this.aselectedTags
      this.addTodo(this.atodo)
    }
  }

  OnSelect(item: any) {
    
  }

  OnDeSelect(item: any) {

  }

  addTagModal(){
    this.closeModal()
    $('#addTagModal').modal('show');
  }

  refreshPage() {
    this.summaryService.setRefresh()
  }
}
