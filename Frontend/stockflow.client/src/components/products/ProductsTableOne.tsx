import { useEffect, useState } from "react";
import { ICategory } from "../../@types/category";
import { IProduct } from "../../@types/product";
import { ISupplier } from "../../@types/supplier";
import { useDeleteModal } from "../../hooks/useDeleteModal";
import { useModal } from "../../hooks/useModal";
import { PencilIcon, TrashBinIcon } from "../../icons";
import { GetCategories, GetCategoryById } from "../../services/categoriesService";
import { DeleteProduct, GetProducts, UpdateProduct } from "../../services/productsService";
import { GetSupplierById, GetSuppliers } from "../../services/suppliersService";
import Label from "../form/Label";
import Select from "../form/Select";
import Input from "../form/input/InputField";
import TextArea from "../form/input/TextArea";
import Button from "../ui/button/Button";
import { Modal } from "../ui/modal";
import {
    Table,
    TableBody,
    TableCell,
    TableHeader,
    TableRow,
} from "../ui/table";

export default function ProductsTableOne() {
    const { isOpen, modalId, openModal, closeModal } = useModal();
    const { isDeleteOpen, deleteModalId, openDeleteModal, closeDeleteModal } = useDeleteModal();
    const [products, setProducts] = useState<IProduct[]>([]);
    const [suppliers, setSuppliers] = useState<{ [key: string]: ISupplier }>({});
    const [categories, setCategories] = useState<{ [key: string]: ICategory }>({});
    const [, setError] = useState<string | null>(null);
    const [editedProduct, setEditedProduct] = useState<Partial<IProduct>>({
        name: "",
        description: "",
        price: 0,
        quantity: 0,
        supplierId: "",
        categoryId: ""
    });
    const [updateSuppliers, setUpdateSuppliers] = useState<{ value: string; label: string }[]>([]);
    const [updateCategories, setUpdateCategories] = useState<{ value: string; label: string }[]>([]);

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                const productsResponse = await GetProducts();
                const productsData = productsResponse.data;
                setProducts(productsData);

                const suppliersMap: { [key: string]: ISupplier } = {};
                const categoriesMap: { [key: string]: ICategory } = {};

                await Promise.all(
                    productsData.map(async (product: IProduct) => {
                        if (product.supplierId && !suppliersMap[product.supplierId]) {
                            const supplierResponse = await GetSupplierById(product.supplierId);
                            suppliersMap[product.supplierId] = supplierResponse.data;
                        }
                        if (product.categoryId && !categoriesMap[product.categoryId]) {
                            const categoryResponse = await GetCategoryById(product.categoryId);
                            categoriesMap[product.categoryId] = categoryResponse.data;
                        }
                    })
                );

                setSuppliers(suppliersMap);
                setCategories(categoriesMap);
            } catch {
                setError("Failed to load products.");
            }
        };

        fetchProducts();
    }, []);

    useEffect(() => {
        if (isOpen && modalId) {
            const selectedProduct = products.find(product => product.id === modalId);
            if (selectedProduct) {
                setEditedProduct({
                    name: selectedProduct.name,
                    description: selectedProduct.description,
                    price: selectedProduct.price,
                    quantity: selectedProduct.quantity,
                    supplierId: selectedProduct.supplierId,
                    categoryId: selectedProduct.categoryId
                });
            }

            const fetchData = async () => {
                try {
                    const suppliersResponse = await GetSuppliers();
                    setUpdateSuppliers(suppliersResponse.data.map((s: any) => ({ value: s.id, label: s.name })));

                    const categoriesResponse = await GetCategories();
                    setUpdateCategories(categoriesResponse.data.map((c: any) => ({ value: c.id, label: c.name })));
                } catch {
                    setError("Failed to load suppliers or categories.");
                }
            };

            fetchData();
        }
    }, [isOpen, modalId, products]);


    const handleEditProduct = async () => {
        if (modalId) {
            const updatedProduct = { ...editedProduct };
            await UpdateProduct(modalId, updatedProduct);
            closeModal();
        }
    };

    const handleDeleteProduct = async () => {
        if (deleteModalId) {
            await DeleteProduct(deleteModalId);
            closeDeleteModal();
        }
    };

    return (
        <div className="overflow-hidden rounded-xl border border-gray-200 bg-white dark:border-white/[0.05] dark:bg-white/[0.03]">
            <div className="max-w-full overflow-x-auto">
                <div className="min-w-[1102px]">
                    <Table>
                        <TableHeader className="border-b border-gray-100 dark:border-white/[0.05]">
                            <TableRow>
                                <TableCell
                                    isHeader
                                    className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400"
                                >
                                    Name And Description
                                </TableCell>
                                <TableCell
                                    isHeader
                                    className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400"
                                >
                                    Category
                                </TableCell>
                                <TableCell
                                    isHeader
                                    className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400"
                                >
                                    Supplier
                                </TableCell>
                                <TableCell
                                    isHeader
                                    className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400"
                                >
                                    Quantity
                                </TableCell>
                                <TableCell
                                    isHeader
                                    className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400"
                                >
                                    Price
                                </TableCell>
                                <TableCell
                                    isHeader
                                    className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400"
                                >
                                    Actions
                                </TableCell>
                            </TableRow>
                        </TableHeader>

                        <TableBody className="divide-y divide-gray-100 dark:divide-white/[0.05]">
                            {products.map((product) => (
                                <TableRow key={product.id}>
                                    <TableCell className="px-5 py-4 sm:px-6 text-start">
                                        <div className="flex items-center gap-3">
                                            <div>
                                                <span className="block font-medium text-gray-800 text-theme-sm dark:text-white/90">
                                                    {product.name}
                                                </span>
                                                <span className="block text-gray-500 text-theme-xs dark:text-gray-400">
                                                    {product.description}
                                                </span>
                                            </div>
                                        </div>
                                    </TableCell>
                                    <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
                                        {categories[product.categoryId]?.name || "N/A"}
                                    </TableCell>
                                    <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
                                        {suppliers[product.supplierId]?.name || "N/A"}
                                    </TableCell>
                                    <TableCell className="px-4 py-3 text-gray-500 text-theme-sm dark:text-gray-400">
                                        {product.quantity}
                                    </TableCell>
                                    <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
                                        {product.price}
                                    </TableCell>
                                    <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
                                        <div className="flexitems-center gap-3">
                                            <div className="flex gap-2">
                                                <button
                                                    onClick={() => openModal(product.id)}
                                                    className="flex w-full items-center justify-center gap-2 rounded-full border border-gray-300 bg-white px-4 py-3 text-sm font-medium text-gray-700 shadow-theme-xs hover:bg-gray-50 hover:text-gray-800 dark:border-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:hover:bg-white/[0.03] dark:hover:text-gray-200 lg:inline-flex lg:w-auto"
                                                >
                                                    <PencilIcon className="fill-gray-500 dark:fill-gray-400 size-5" />
                                                </button>
                                                <button
                                                    onClick={() => openDeleteModal(product.id)}
                                                    className="flex w-full items-center justify-center gap-2 rounded-full border border-gray-300 bg-white px-4 py-3 text-sm font-medium text-gray-700 shadow-theme-xs hover:bg-gray-50 hover:text-gray-800 dark:border-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:hover:bg-white/[0.03] dark:hover:text-gray-200 lg:inline-flex lg:w-auto"
                                                >
                                                    <TrashBinIcon className="fill-gray-500 dark:fill-gray-400 size-5" />
                                                </button>
                                            </div>
                                        </div>
                                    </TableCell>
                                    <Modal isOpen={isOpen} onClose={closeModal} className="max-w-[700px] m-4">
                                        <div className="no-scrollbar relative w-full max-w-[700px] overflow-y-auto rounded-3xl bg-white p-4 dark:bg-gray-900 lg:p-11">
                                            <div className="px-2 pr-14">
                                                <h4 className="mb-2 text-2xl font-semibold text-gray-800 dark:text-white/90">
                                                    Edit Product Information
                                                </h4>
                                                <p className="mb-6 text-sm text-gray-500 dark:text-gray-400 lg:mb-7">
                                                    Update your product details.
                                                </p>
                                            </div>
                                            <form className="flex flex-col">
                                                <div className="custom-scrollbar h-[450px] overflow-y-auto px-2 pb-3">
                                                    <div className="mt-7">
                                                        <h5 className="mb-5 text-lg font-medium text-gray-800 dark:text-white/90 lg:mb-6">
                                                            Product Information
                                                        </h5>

                                                        <div className="grid grid-cols-1 gap-x-6 gap-y-5 lg:grid-cols-2">
                                                            <div className="col-span-2 lg:col-span-2">
                                                                <Label>Name</Label>
                                                                <Input
                                                                    type="text"
                                                                    value={editedProduct.name || product?.name}
                                                                    onChange={(e) => setEditedProduct({ ...editedProduct, name: e.target.value })}
                                                                />
                                                            </div>

                                                            <div className="col-span-2">
                                                                <Label>Description</Label>
                                                                <TextArea
                                                                    value={editedProduct.description || product?.description}
                                                                    onChange={(value) => setEditedProduct({ ...editedProduct, description: value })}
                                                                    rows={6}
                                                                />
                                                            </div>

                                                            <div className="col-span-2 lg:col-span-1">
                                                                <div className="grid grid-cols-2 gap-4">
                                                                    <div>
                                                                        <Label>Price</Label>
                                                                        <Input
                                                                            type="number"
                                                                            value={editedProduct.price || product?.price}
                                                                            onChange={(e) => setEditedProduct({ ...editedProduct, price: Number(e.target.value) })}
                                                                        />
                                                                    </div>

                                                                    <div>
                                                                        <Label>Quantity</Label>
                                                                        <Input
                                                                            type="number"
                                                                            value={editedProduct.quantity || product?.quantity}
                                                                            onChange={(e) => setEditedProduct({ ...editedProduct, quantity: Number(e.target.value) })}
                                                                        />
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div className="col-span-2 lg:col-span-1">
                                                                <Label>Supplier</Label>
                                                                <div className="relative">
                                                                    <Select
                                                                        options={updateSuppliers}
                                                                        placeholder="Select a supplier"
                                                                        onChange={(value) => setEditedProduct({ ...editedProduct, supplierId: value })}
                                                                        className="dark:bg-dark-900"
                                                                    />
                                                                </div>
                                                            </div>

                                                            <div className="col-span-2 lg:col-span-1">
                                                                <Label>Category</Label>
                                                                <div className="relative">
                                                                    <Select
                                                                        options={updateCategories}
                                                                        placeholder="Select a category"
                                                                        onChange={(value) => setEditedProduct({ ...editedProduct, categoryId: value })}
                                                                        className="dark:bg-dark-900"
                                                                    />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div className="flex items-center gap-3 px-2 mt-6 lg:justify-end">
                                                    <Button size="sm" variant="outline" onClick={closeModal}>
                                                        Close
                                                    </Button>
                                                    <Button size="sm" onClick={handleEditProduct}>
                                                        Save Changes
                                                    </Button>
                                                </div>
                                            </form>
                                        </div>
                                    </Modal>
                                    <Modal isOpen={isDeleteOpen} onClose={closeDeleteModal} className="max-w-[700px] m-4">
                                        <div className="no-scrollbar relative w-full max-w-[700px] overflow-y-auto rounded-3xl bg-white p-4 dark:bg-gray-900 lg:p-11">
                                            <div className="px-2 pr-14 text-center">
                                                <h4 className="mb-2 text-2xl font-semibold text-gray-800 dark:text-white/90">
                                                    Delete Product
                                                </h4>
                                                <p className="mb-6 text-sm text-gray-500 dark:text-gray-400 lg:mb-7">
                                                    Are you sure you want to delete this product?
                                                </p>
                                            </div>
                                            <form className="flex flex-col">
                                                <div className="flex items-center gap-3 px-2 mt-6 lg:justify-center">
                                                    <Button size="sm" onClick={handleDeleteProduct}>
                                                        Yes, delete
                                                    </Button>
                                                    <Button size="sm" variant="outline" onClick={closeDeleteModal}>
                                                        No
                                                    </Button>
                                                </div>
                                            </form>
                                        </div>
                                    </Modal>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </div>
            </div>
        </div>
    );
}
