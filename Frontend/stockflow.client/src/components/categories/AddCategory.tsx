import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { ICategory } from "../../@types/category";
import { CreateCategory } from "../../services/categoriesService";
import { showErrorToast, showSuccessToast } from "../../utils/toastHelper";
import ComponentCard from "../common/ComponentCard";
import Input from "../form/input/InputField";
import Label from "../form/Label";
import Button from "../ui/button/Button";

export default function AddCategory() {
    const [name, setName] = useState("");
    const navigate = useNavigate();

    const handleCategoryCreation = async (e: React.FormEvent) => {
        e.preventDefault();

        const newCategory: ICategory = { name };

        try {
            await CreateCategory(newCategory);
            showSuccessToast();

            navigate("/categories");
        } catch {
            showErrorToast();
        }
    }

    return (
        <ComponentCard title="Add Product">
            <form onSubmit={handleCategoryCreation}>
                <div className="space-y-6">
                    <div>
                        <Label htmlFor="input">Name</Label>
                        <Input type="text" value={name} onChange={(e) => setName(e.target.value)} />
                    </div>
                    <div className="text-center">
                        <Button className="w-sm" size="sm">
                            Create Category
                        </Button>
                    </div>
                </div>
            </form>
        </ComponentCard>
    );
}
