import { ICustomer } from "../@types/customer";
import apiRequest from "./helpers/apiService";

export const GetCustomers = async () => apiRequest("GET", "customers", undefined, true);

export const GetCustomerById = async (customerId: string) => apiRequest("GET", `customers/${customerId}`, undefined, true);

export const CreateCustomer = async (newCustomer: ICustomer) => apiRequest("POST", "customers", newCustomer, true);