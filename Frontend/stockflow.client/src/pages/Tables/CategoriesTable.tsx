import { useNavigate } from "react-router-dom";
import CategoriesTableOne from "../../components/categories/CategoriesTableOne";
import ComponentCard from "../../components/common/ComponentCard";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import PageMeta from "../../components/common/PageMeta";

export default function CategoriesTable() {
    const navigate = useNavigate();

    return (
        <>
            <PageMeta
                title="Admin Categories Table"
                description="This is the admin categories table to see all the categories"
            />
            <PageBreadcrumb pageTitle="Categories" />
            <div className="space-y-6">
                <ComponentCard title="">
                    <div className="flex items-center justify-between">
                        <h2 className="text-lg font-semibold text-gray-700 dark:text-gray-400">All Categories</h2>
                        <button
                            onClick={() => navigate("/addcategory")}
                            className="inline-flex items-center gap-2 rounded-lg border border-gray-300 bg-white px-4 py-2.5 text-theme-sm font-medium text-gray-700 shadow-theme-xs hover:bg-gray-50 hover:text-gray-800 dark:border-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:hover:bg-white/[0.03] dark:hover:text-gray-200">
                            Add Category
                        </button>
                    </div>
                    <CategoriesTableOne />
                </ComponentCard>
            </div>
        </>
    );
}
