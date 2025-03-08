import { IPurchase } from "../@types/purchase";
import apiRequest from "./helpers/apiService";

export const GetPurchases = async () => apiRequest("GET", "purchases", undefined, true);

export const CreatePurchase = async (newPurchase: IPurchase) => apiRequest("POST", "purchases", newPurchase, true);