import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { IPurchase } from "../../@types/purchase";
import { IPurchaseItem } from "../../@types/purchaseItem";
import { GetProducts } from "../../services/productsService";
import { CreatePurchaseItem } from "../../services/purchaseItemsService";
import { GetSuppliers } from "../../services/suppliersService";
import { showErrorToast, showSuccessToast } from "../../utils/toastHelper";
import ComponentCard from "../common/ComponentCard";
import Input from "../form/input/InputField";
import Label from "../form/Label";
import Select from "../form/Select";
import Button from "../ui/button/Button";
import { CreatePurchase } from "../../services/purchasesService";
import { CalenderIcon } from "../../icons";

export default function AddPurchase() {
    // Purchase Item
    const [productId, setProductId] = useState("");
    const [quantity, setQuantity] = useState<number | "">("");
    const [unitPrice, setUnitPrice] = useState<number>(0);
    const [subTotal, setSubTotal] = useState<number>(0);

    // Purchase
    const [supplierId, setSupplierId] = useState("");
    const [purchaseDate, setPurchaseDate] = useState("");

    const [suppliers, setSuppliers] = useState<{ value: string; label: string }[]>([]);
    const [products, setProducts] = useState<{ value: string; label: string; price: number }[]>([]);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchData = async () => {
            try {
                const suppliersResponse = await GetSuppliers();
                setSuppliers(suppliersResponse.data.map((s: any) => ({ value: s.id, label: s.name })));

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

    const handlePurchaseCreation = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!productId || !supplierId || !quantity || !unitPrice || !purchaseDate) {
            showErrorToast();
            return;
        }

        const newPurchaseItem: IPurchaseItem = {
            purchaseId: "",
            productId,
            quantity: Number(quantity),
            unitPrice,
            subTotal: subTotal
        };

        const newPurchase: IPurchase = {
            supplierId,
            purchaseDate,
            total: subTotal
        };

        try {
            const purchaseResponse = await CreatePurchase(newPurchase);
            newPurchaseItem.purchaseId = purchaseResponse.data.id;

            await CreatePurchaseItem(newPurchaseItem);
            showSuccessToast();

            navigate("/products");
        } catch {
            showErrorToast();
        }
    };

    return (
        <ComponentCard title="Add Purchase">
            <form onSubmit={handlePurchaseCreation}>
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
                        <Label>Supplier</Label>
                        <Select
                            options={suppliers}
                            placeholder="Select a supplier"
                            onChange={setSupplierId}
                            className="dark:bg-dark-900"
                        />
                    </div>
                    <div>
                        <Label htmlFor="input">Purchase Date</Label>
                        <div className="relative">
                            <Input type="date" value={purchaseDate} onChange={(e) => setPurchaseDate(e.target.value)} />
                            <span className="absolute text-gray-500 -translate-y-1/2 pointer-events-none right-3 top-1/2 dark:text-gray-400">
                                <CalenderIcon className="size-6" />
                            </span>
                        </div>
                    </div>
                    <div className="text-center">
                        <Button className="w-sm" size="sm">
                            Create Purchase
                        </Button>
                    </div>
                </div>
            </form>
        </ComponentCard>
    );
}
