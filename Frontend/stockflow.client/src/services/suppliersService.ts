import { ISupplier } from "../@types/supplier";
import apiRequest from "./helpers/apiService";

export const GetSuppliers = async () => apiRequest("GET", "suppliers", undefined, true);

export const GetSupplierById = async (supplierId: string) => apiRequest("GET", `suppliers/${supplierId}`, undefined, true);

export const CreateSupplier = async (newSupplier: ISupplier) => apiRequest("POST", "suppliers", newSupplier, true);

export const UpdateSupplier = async (supplierId: string, updateSupplier: Partial<ISupplier>) =>
    apiRequest("PUT", `suppliers/${supplierId}`, updateSupplier, true);

export const DeleteSupplier = async (supplierId: string) => apiRequest("DELETE", `suppliers/${supplierId}`, undefined, true);