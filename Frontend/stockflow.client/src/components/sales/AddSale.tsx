import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { ISale } from "../../@types/sale";
import { ISaleItem } from "../../@types/saleItem";
import { CalenderIcon } from "../../icons";
import { GetCustomers } from "../../services/customersService";
import { GetProducts } from "../../services/productsService";
import { CreateSaleItem } from "../../services/saleItemsService";
import { CreateSale } from "../../services/salesService";
import { showErrorToast, showSuccessToast } from "../../utils/toastHelper";
import ComponentCard from "../common/ComponentCard";
import Input from "../form/input/InputField";
import Label from "../form/Label";
import Select from "../form/Select";
import Button from "../ui/button/Button";

export default function AddSale() {
    // Sale Item
    const [productId, setProductId] = useState("");
    const [quantity, setQuantity] = useState<number | "">("");
    const [unitPrice, setUnitPrice] = useState<number>(0);
    const [subTotal, setSubTotal] = useState<number>(0);

    // Sale
    const [customerId, setCustomerId] = useState("");
    const [saleDate, setSaleDate] = useState("");

    const [customers, setCustomers] = useState<{ value: string; label: string }[]>([]);
    const [products, setProducts] = useState<{ value: string; label: string; price: number }[]>([]);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchData = async () => {
            try {
                const customersResponse = await GetCustomers();
                setCustomers(customersResponse.data.map((s: any) => ({ value: s.id, label: s.name })));

                const productsResponse = await GetProducts();
                setProducts(productsResponse.data.map((p: any) => ({ value: p.id, label: p.name, price: p.price })));
            } catch {
                showErrorToast();
            }
        };

        fetchData();
    }, []);

    useEffect(() => {
        const selectedProduct = products.find((p) => p.value === productId);
        if (selectedProduct) {
            setUnitPrice(selectedProduct.price);
        } else {
            setUnitPrice(0);
        }
    }, [productId, products]);

    useEffect(() => {
        if (quantity && unitPrice) {
            setSubTotal(quantity * unitPrice);
        } else {
            setSubTotal(0);
        }
    }, [quantity, unitPrice]);

    const handleSaleCreation = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!productId || !customerId || !quantity || !unitPrice || !saleDate) {
            showErrorToast();
            return;
        }

        const newSaleItem: ISaleItem = {
            saleId: "",
            productId,
            quantity: Number(quantity),
            unitPrice,
            subTotal: subTotal
        };

        const newSale: ISale = {
            customerId,
            saleDate,
            total: subTotal
        };

        try {
            const saleResponse = await CreateSale(newSale);
            newSaleItem.saleId = saleResponse.data.id;

            await CreateSaleItem(newSaleItem);
            showSuccessToast();

            navigate("/sales");
        } catch {
            showErrorToast();
        }
    };

    return (
        <ComponentCard title="Add Sale">
            <form onSubmit={handleSaleCreation}>
                <div className="space-y-6">
                    <div>
                        <Label>Product</Label>
                        <Select
                            options={products}
                            placeholder="Select a product"
                            onChange={setProductId}
                            className="dark:bg-dark-900"
                        />
                    </div>
                    <div>
                        <Label htmlFor="input">Quantity</Label>
                        <Input type="number" value={quantity} onChange={(e) => setQuantity(Number(e.target.value))} />
                    </div>
                    <div>
                        <Label htmlFor="input">Unit Price</Label>
                        <Input type="text" value={unitPrice} disabled />
                    </div>
                    <div>
                        <Label>Subtotal</Label>
                        <Input type="text" value={subTotal} disabled />
                    </div>
                    <div>
                        <Label>Customer</Label>
                        <Select
                            options={customers}
                            placeholder="Select a customer"
                            onChange={setCustomerId}
                            className="dark:bg-dark-900"
                        />
                    </div>
                    <div>
                        <Label htmlFor="input">Sale Date</Label>
                        <div className="relative">
                            <Input type="date" value={saleDate} onChange={(e) => setSaleDate(e.target.value)} />
                            <span className="absolute text-gray-500 -translate-y-1/2 pointer-events-none right-3 top-1/2 dark:text-gray-400">
                                <CalenderIcon className="size-6" />
                            </span>
                        </div>
                    </div>
                    <div className="text-center">
                        <Button className="w-sm" size="sm">
                            Create Sale
                        </Button>
                    </div>
                </div>
            </form>
        </ComponentCard>
    );
}
