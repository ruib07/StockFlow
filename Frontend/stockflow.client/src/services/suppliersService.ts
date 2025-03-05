import apiRequest from "./helpers/apiService";

export const GetSuppliers = async () => apiRequest("GET", "suppliers", undefined, true);