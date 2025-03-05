import { useEffect, useState } from "react";
import { GroupIcon } from "../../icons";
import { GetCustomers } from "../../services/customersService";
import { GetSuppliers } from "../../services/suppliersService";

export default function Metrics() {
    const [customersCount, setCustomersCount] = useState(0);
    const [suppliersCount, setSuppliersCount] = useState(0);
    const [, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchCustomersAndSuppliers = async () => {
            try {
                const customersResponse = await GetCustomers();
                setCustomersCount(customersResponse.data.length);

                const suppliersResponse = await GetSuppliers();
                setSuppliersCount(suppliersResponse.data.length);
            } catch {
                setError("Failed to load customers and suppliers.");
            }
        };

        fetchCustomersAndSuppliers();
    }, []);

    return (
        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 md:gap-6">
            <div className="rounded-2xl border border-gray-200 bg-white p-5 dark:border-gray-800 dark:bg-white/[0.03] md:p-6">
                <div className="flex items-center justify-center w-12 h-12 bg-gray-100 rounded-xl dark:bg-gray-800">
                    <GroupIcon className="text-gray-800 size-6 dark:text-white/90" />
                </div>

                <div className="flex items-end justify-between mt-5">
                    <div>
                        <span className="text-sm text-gray-500 dark:text-gray-400">
                            Customers
                        </span>
                        <h4 className="mt-2 font-bold text-gray-800 text-title-sm dark:text-white/90">
                            {customersCount}
                        </h4>
                    </div>
                </div>
            </div>
            <div className="rounded-2xl border border-gray-200 bg-white p-5 dark:border-gray-800 dark:bg-white/[0.03] md:p-6">
                <div className="flex items-center justify-center w-12 h-12 bg-gray-100 rounded-xl dark:bg-gray-800">
                    <GroupIcon className="text-gray-800 size-6 dark:text-white/90" />
                </div>
                <div className="flex items-end justify-between mt-5">
                    <div>
                        <span className="text-sm text-gray-500 dark:text-gray-400">
                            Suppliers
                        </span>
                        <h4 className="mt-2 font-bold text-gray-800 text-title-sm dark:text-white/90">
                            {suppliersCount}
                        </h4>
                    </div>
                </div>
            </div>
        </div>
    );
}
