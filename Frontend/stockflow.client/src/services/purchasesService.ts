import apiRequest from "./helpers/apiService";

export const GetPurchases = async () => apiRequest("GET", "purchases", undefined, true);