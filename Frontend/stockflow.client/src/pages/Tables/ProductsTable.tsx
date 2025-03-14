import { useNavigate } from "react-router-dom";
import ComponentCard from "../../components/common/ComponentCard";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import PageMeta from "../../components/common/PageMeta";
import ProductsTableOne from "../../components/products/ProductsTableOne";

export default function ProductsTable() {
    const navigate = useNavigate();

    return (
        <>
            <PageMeta
                title="Admin Products Table"
                description="This is the admin products table to see all the products"
            />
            <PageBreadcrumb pageTitle="Products" />
            <div className="space-y-6">
                <ComponentCard title="">
                    <div className="flex items-center justify-between">
                        <h2 className="text-lg font-semibold text-gray-700 dark:text-gray-400">All Products</h2>
                        <div>
                            <button
                                onClick={() => navigate("/addproduct")}
                                className="inline-flex items-center gap-2 rounded-lg border border-gray-300 bg-white px-4 py-2.5 text-theme-sm font-medium text-gray-700 shadow-theme-xs hover:bg-gray-50 hover:text-gray-800 dark:border-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:hover:bg-white/[0.03] dark:hover:text-gray-200">
                                Add Product
                            </button>
                        </div>
                    </div>
                    <ProductsTableOne />
                </ComponentCard>
            </div>
        </>
    );
}
