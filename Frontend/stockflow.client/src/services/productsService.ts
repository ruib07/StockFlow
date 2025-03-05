import apiRequest from "./helpers/apiService";

export const GetProducts = async () => apiRequest("GET", "products", undefined, true);