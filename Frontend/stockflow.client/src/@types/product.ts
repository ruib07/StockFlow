export interface IProduct {
    id?: string;
    name: string;
    description: string;
    price: number;
    quantity: number;
    supplierId: string;
    categoryId: string;
    createdAt?: string;
}