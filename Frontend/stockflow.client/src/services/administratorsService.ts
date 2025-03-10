import { IAdministrator } from "../@types/administrator";
import apiRequest from "./helpers/apiService";

export const GetAdminById = async (adminId: string) => apiRequest("GET", `administrators/${adminId}`, undefined, true);

export const UpdateAdmin = async (adminId: string, updateAdmin: Partial<IAdministrator>) =>
    apiRequest("PUT", `administrators/${adminId}`, updateAdmin, true);

export const DeleteAccount = async (adminId: string) => apiRequest("DELETE", `administrators/${adminId}`, undefined, true);