import { useNavigate } from "react-router-dom";
import ComponentCard from "../../components/common/ComponentCard";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import PageMeta from "../../components/common/PageMeta";
import PurchasesTableOne from "../../components/purchases/PurchasesTableOne";

export default function PurchasesTable() {
    const navigate = useNavigate();

    return (
        <>
            <PageMeta
                title="Admin Purchases Table"
                description="This is the admin purchases table to see all the purchases"
            />
            <PageBreadcrumb pageTitle="Purchases" />
            <div className="space-y-6">
                <ComponentCard title="">
                    <div className="flex items-center justify-between">
                        <h2 className="text-lg font-semibold text-gray-700 dark:text-gray-400">All Purchases</h2>
                        <button
                            onClick={() => navigate("/addpurchase")}
                            className="inline-flex items-center gap-2 rounded-lg border border-gray-300 bg-white px-4 py-2.5 text-theme-sm font-medium text-gray-700 shadow-theme-xs hover:bg-gray-50 hover:text-gray-800 dark:border-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:hover:bg-white/[0.03] dark:hover:text-gray-200">
                            Add Purchase
                        </button>
                    </div>
                    <PurchasesTableOne />
                </ComponentCard>
            </div>
        </>
    );
}
