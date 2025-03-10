import { useNavigate } from "react-router-dom";
import ComponentCard from "../../components/common/ComponentCard";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import PageMeta from "../../components/common/PageMeta";
import SalesTableOne from "../../components/sales/SalesTableOne";

export default function SalesTable() {
    const navigate = useNavigate();

    return (
        <>
            <PageMeta
                title="Admin Sales Table"
                description="This is the admin sales table to see all the sales"
            />
            <PageBreadcrumb pageTitle="Sales" />
            <div className="space-y-6">
                <ComponentCard title="">
                    <div className="flex items-center justify-between">
                        <h2 className="text-lg font-semibold text-gray-700 dark:text-gray-400">All Sales</h2>
                        <button
                            onClick={() => navigate("/addsale")}
                            className="inline-flex items-center gap-2 rounded-lg border border-gray-300 bg-white px-4 py-2.5 text-theme-sm font-medium text-gray-700 shadow-theme-xs hover:bg-gray-50 hover:text-gray-800 dark:border-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:hover:bg-white/[0.03] dark:hover:text-gray-200">
                            Add Sale
                        </button>
                    </div>
                    <SalesTableOne />
                </ComponentCard>
            </div>
        </>
    );
}
