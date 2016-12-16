export class User {
    public Id: number;
    public Name: string;
    public Password: string;
    public ConfirmPassword: string;
    public Token: string;
    
    constructor(id?: number, name?: string) {
        this.Id = id;
        this.Name = name;
    }
}