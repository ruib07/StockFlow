import apiRequest from "./helpers/apiService";

export const GetSales = async () => apiRequest("GET", "sales", undefined, true);