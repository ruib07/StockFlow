import apiRequest from "./helpers/apiService";

export const GetSuppliers = async () => apiRequest("GET", "suppliers", undefined, true);

export const GetSupplierById = async (supplierId: string) => apiRequest("GET", `suppliers/${supplierId}`, undefined, true);