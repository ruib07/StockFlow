import { IProduct } from "../@types/product";
import apiRequest from "./helpers/apiService";

export const GetProducts = async () => apiRequest("GET", "products", undefined, true);

export const CreateProduct = async (newProduct: IProduct) => apiRequest("POST", "products", newProduct, true);

export const UpdateProduct = async (productId: string, updateProduct: Partial<IProduct>) =>
    apiRequest("PUT", `products/${productId}`, updateProduct, true);

export const DeleteProduct = async (productId: string) => apiRequest("DELETE", `products/${productId}`, undefined, true);