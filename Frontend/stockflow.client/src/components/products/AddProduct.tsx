import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { IProduct } from "../../@types/product";
import { GetCategories } from "../../services/categoriesService";
import { CreateProduct } from "../../services/productsService";
import { GetSuppliers } from "../../services/suppliersService";
import { showErrorToast, showSuccessToast } from "../../utils/toastHelper";
import ComponentCard from "../common/ComponentCard";
import Input from "../form/input/InputField";
import TextArea from "../form/input/TextArea";
import Label from "../form/Label";
import Select from "../form/Select";
import Button from "../ui/button/Button";

export default function AddProduct() {
    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [price, setPrice] = useState<number | "">("");
    const [quantity, setQuantity] = useState<number | "">("");
    const [supplierId, setSupplierId] = useState("");
    const [categoryId, setCategoryId] = useState("");
    const [suppliers, setSuppliers] = useState<{ value: string; label: string }[]>([]);
    const [categories, setCategories] = useState<{ value: string; label: string }[]>([]);
    const [, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchData = async () => {
            try {
                const suppliersResponse = await GetSuppliers();
                setSuppliers(suppliersResponse.data.map((s: any) => ({ value: s.id, label: s.name })));

                const categoriesResponse = await GetCategories();
                setCategories(categoriesResponse.data.map((c: any) => ({ value: c.id, label: c.name })));
            } catch {
                setError("Failed to load data.");
            }
        };

        fetchData();
    });

    const handleProductCreation = async (e: React.FormEvent) => {
        e.preventDefault();

        const newProduct: IProduct = {
            name,
            description,
            price: Number(price),
            quantity: Number(quantity),
            supplierId,
            categoryId
        };

        try {
            await CreateProduct(newProduct);
            showSuccessToast();

            navigate("/products");
        } catch {
            showErrorToast();
        }
    }

    return (
        <ComponentCard title="Add Product">
            <form onSubmit={handleProductCreation}>
                <div className="space-y-6">
                    <div>
                        <Label htmlFor="input">Name</Label>
                        <Input type="text" value={name} onChange={(e) => setName(e.target.value)} />
                    </div>
                    <div>
                        <Label>Description</Label>
                        <TextArea
                            value={description}
                            onChange={setDescription}
                            rows={6}
                        />
                    </div>
                    <div>
                        <Label htmlFor="input">Price</Label>
                        <Input type="number" value={price} onChange={(e) => setPrice(Number(e.target.value))} />
                    </div>
                    <div>
                        <Label htmlFor="input">Quantity</Label>
                        <Input type="number" value={quantity} onChange={(e) => setQuantity(Number(e.target.value))} />
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
                        <Label>Category</Label>
                        <Select
                            options={categories}
                            placeholder="Select a category"
                            onChange={setCategoryId}
                            className="dark:bg-dark-900"
                        />
                    </div>
                    <div className="text-center">
                        <Button className="w-sm" size="sm">
                            Create Product
                        </Button>
                    </div>
                </div>
            </form>
        </ComponentCard>
    );
}
