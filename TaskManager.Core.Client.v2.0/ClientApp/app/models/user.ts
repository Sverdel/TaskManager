export class User {
    public id: string;
    public name: string;
    public password: string;
    public confirmPassword: string;
    public token: string;
    public accessToken: string;
    public tokenExpireDate: Date;

    constructor() {
    }
}