import apiRequest from "./helpers/apiService";

export const GetCategories = async () => apiRequest("GET", "categories", undefined, true);