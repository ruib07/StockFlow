import apiRequest from "./helpers/apiService";

export const GetAdminById = async (adminId: string) => apiRequest("GET", `administrators/${adminId}`, undefined, true);