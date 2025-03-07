import { ISupplier } from "../@types/supplier";
import apiRequest from "./helpers/apiService";

export const GetSuppliers = async () => apiRequest("GET", "suppliers", undefined, true);

export const GetSupplierById = async (supplierId: string) => apiRequest("GET", `suppliers/${supplierId}`, undefined, true);

export const CreateSupplier = async (newSupplier: ISupplier) => apiRequest("POST", "suppliers", newSupplier, true);