import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ApiService } from 'src/app/common/api.service';
import { Tag } from 'src/app/model/tag';
import { SummaryService } from 'src/app/summary/summary.service';
import { AddTagService } from './add-tag.service';

@Component({
  selector: 'app-add-tag',
  templateUrl: './add-tag.component.html',
  styleUrls: ['./add-tag.component.scss']
})
export class AddTagComponent implements OnInit {

  public tag_name:string = ''
  
  constructor(private apiService: ApiService, private summaryService: SummaryService, private addTagService: AddTagService) { }

  ngOnInit(): void {
  }

  closeModal(){
    $('#addTagModal').modal('hide');
  }

  public onConfirm(f: NgForm) {
    if (f.valid && this.tag_name){
      
      this.addTag(new Tag(0,this.tag_name))
    }
  }

  addTag(tag: Tag){
    
    this.apiService.AddTag(tag)
      .subscribe(
        (response: any) => {
          this.closeModal()
          this.refreshPage()
        });
  }
  
  refreshPage() {
    this.addTagService.setRefresh()
  }

}
