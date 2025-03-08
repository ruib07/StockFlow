export interface ISaleItem {
    id?: string;
    saleId: string;
    productId: string;
    quantity: number;
    unitPrice: number;
    subTotal: number;
}