export interface IPurchaseItem {
    id?: string;
    purchaseId: string;
    productId: string;
    quantity: number;
    unitPrice: number;
    subTotal: number;
}