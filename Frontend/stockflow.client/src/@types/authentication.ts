export interface ISignup {
    name: string;
    email: string;
    password: string;
}

export interface ISignin {
    email: string;
    password: string;
}

export interface ISendEmail {
    email: string;
}

export interface IChangePassword {
    token: string;
    newPassword: string;
    confirmNewPassword: string;
}