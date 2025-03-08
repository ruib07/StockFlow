import { IChangePassword, ISendEmail } from "../@types/authentication";
import apiRequest from "./helpers/apiService";

export const SendEmail = async (sendEmail: ISendEmail) => apiRequest("POST", "administrators/recoverpassword", sendEmail, false);

export const UpdatePassword = async (changePassword: IChangePassword) => apiRequest("PUT", "administrators/updatepassword", changePassword, false);