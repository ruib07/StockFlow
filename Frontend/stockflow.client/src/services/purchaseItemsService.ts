import { IPurchaseItem } from "../@types/purchaseItem";
import apiRequest from "./helpers/apiService";

export const CreatePurchaseItem = async (newPurchaseItem: IPurchaseItem) => apiRequest("POST", "purchaseitems", newPurchaseItem, true);