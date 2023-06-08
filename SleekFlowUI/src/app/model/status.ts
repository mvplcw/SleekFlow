export class Status {
    public status_id: number;
    public status_name: string;

    constructor(status_id: number, status_name: string){
        this.status_id = status_id;
        this.status_name = status_name;
    }
}