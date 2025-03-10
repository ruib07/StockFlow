import { ICustomer } from "../@types/customer";
import apiRequest from "./helpers/apiService";

export const GetCustomers = async () => apiRequest("GET", "customers", undefined, true);

export const GetCustomerById = async (customerId: string) => apiRequest("GET", `customers/${customerId}`, undefined, true);

export const CreateCustomer = async (newCustomer: ICustomer) => apiRequest("POST", "customers", newCustomer, true);

export const UpdateCustomer = async (customerId: string, updateCustomer: Partial<ICustomer>) =>
    apiRequest("PUT", `customers/${customerId}`, updateCustomer, true);

export const DeleteCustomer = async (customerId: string) => apiRequest("DELETE", `customers/${customerId}`, undefined, true);