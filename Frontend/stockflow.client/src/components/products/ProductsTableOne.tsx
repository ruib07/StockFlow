import {
    Table,
    TableBody,
    TableCell,
    TableHeader,
    TableRow,
} from "../ui/table";
import { useEffect, useState } from "react";
import { IProduct } from "../../@types/product";
import { GetProducts } from "../../services/productsService";
import { ICategory } from "../../@types/category";
import { ISupplier } from "../../@types/supplier";
import { GetSupplierById } from "../../services/suppliersService";
import { GetCategoryById } from "../../services/categoriesService";

export default function ProductsTableOne() {
    const [products, setProducts] = useState<IProduct[]>([]);
    const [suppliers, setSuppliers] = useState<{ [key: string]: ISupplier }>({});
    const [categories, setCategories] = useState<{ [key: string]: ICategory }>({});
    const [, setError] = useState<string | null>(null);

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
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </div>
            </div>
        </div>
    );
}
