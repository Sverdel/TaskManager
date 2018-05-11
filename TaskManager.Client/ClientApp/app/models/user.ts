export class User {
    public id: string;
    public userName: string;
    public password: string;
    public confirmPassword: string;
    public accessToken: string;
    public tokenExpireDate: Date;

    constructor() {
    }
}