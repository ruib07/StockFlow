import { ISale } from "../@types/sale";
import apiRequest from "./helpers/apiService";

export const GetSales = async () => apiRequest("GET", "sales", undefined, true);

export const CreateSale = async (newSale: ISale) => apiRequest("POST", "sales", newSale, true);