import { ICategory } from "../@types/category";
import apiRequest from "./helpers/apiService";

export const GetCategories = async () => apiRequest("GET", "categories", undefined, true);

export const GetCategoryById = async (categoryId: string) => apiRequest("GET", `categories/${categoryId}`, undefined, true);

export const CreateCategory = async (newCategory: ICategory) => apiRequest("POST", "categories", newCategory, true);