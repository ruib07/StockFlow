import { ISaleItem } from "../@types/saleItem";
import apiRequest from "./helpers/apiService";

export const CreateSaleItem = async (newSaleItem: ISaleItem) => apiRequest("POST", "saleitems", newSaleItem, true);