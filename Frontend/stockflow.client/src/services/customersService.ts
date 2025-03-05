import apiRequest from "./helpers/apiService";

export const GetCustomers = async () => apiRequest("GET", "customers", undefined, true);