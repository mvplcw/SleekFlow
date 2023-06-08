import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AppComponent } from './app.component';
import { TopNavComponent } from './top-nav/top-nav.component';
import { SummaryComponent } from './summary/summary.component';
import { AppRoutingModule } from './app-routing.module';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { SummaryModule } from './summary/summary.module';
import { CUSTOM_ELEMENTS_SCHEMA,NO_ERRORS_SCHEMA} from '@angular/core';
import { AddTodoComponent } from './modal/add-todo/add-todo.component';
import { ViewTodoComponent } from './modal/view-todo/view-todo.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';

import * as bootstrap from "bootstrap";
import * as $ from "jquery";
import { AddTagComponent } from './modal/add-tag/add-tag.component';

const routes: Routes = [
  { path: '', redirectTo: '/summary', pathMatch: 'full' },
  { path: 'summary', component: SummaryComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    TopNavComponent,
    AddTodoComponent,
    ViewTodoComponent,
    AddTagComponent
  ],
  imports: [
    SummaryModule,
    CommonModule,
    AppRoutingModule,
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot(routes, { useHash: true,anchorScrolling:'enabled' }),
    RouterModule.forChild(routes),
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    FormsModule,
    ReactiveFormsModule,
    NgMultiSelectDropDownModule
  ],
  schemas: [ CUSTOM_ELEMENTS_SCHEMA,NO_ERRORS_SCHEMA ],
  exports: [RouterModule],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
