import { IProduct } from "../@types/product";
import apiRequest from "./helpers/apiService";

export const GetProducts = async () => apiRequest("GET", "products", undefined, true);

export const CreateProduct = async (newProduct: IProduct) => apiRequest("POST", "products", newProduct, true);