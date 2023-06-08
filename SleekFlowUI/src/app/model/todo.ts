import { Tag } from "./tag";

export class Todo {
    public todo_id: number;
    public todo_name: string;
    public todo_description: string;
    public todo_status:string;
    public todo_due_date: string;
    public todo_tags: Tag[];

    constructor(todo_id: number, todo_name: string, todo_description: string,todo_status:string, todo_due_date: string, todo_tags: Tag[]){
        this.todo_id = todo_id;
        this.todo_name = todo_name;
        this.todo_description = todo_description;
        this.todo_status = todo_status;
        this.todo_due_date = todo_due_date;
        this.todo_tags = todo_tags
    }
}