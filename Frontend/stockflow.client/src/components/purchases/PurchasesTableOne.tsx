import { useEffect, useState } from "react";
import { IPurchase } from "../../@types/purchase";
import { ISupplier } from "../../@types/supplier";
import { GetPurchases } from "../../services/purchasesService";
import { GetSupplierById } from "../../services/suppliersService";
import {
    Table,
    TableBody,
    TableCell,
    TableHeader,
    TableRow,
} from "../ui/table";

export default function PurchasesTableOne() {
    const [purchases, setPurchases] = useState<IPurchase[]>([]);
    const [suppliers, setSuppliers] = useState<{ [key: string]: ISupplier }>({});
    const [, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchPurchases = async () => {
            try {
                const purchasesResponse = await GetPurchases();
                const purchasesData = purchasesResponse.data;
                setPurchases(purchasesData);

                const suppliersMap: { [key: string]: ISupplier } = {};

                await Promise.all(
                    purchasesData.map(async (purchase: IPurchase) => {
                        if (purchase.supplierId && !suppliersMap[purchase.supplierId]) {
                            const supplierResponse = await GetSupplierById(purchase.supplierId);
                            suppliersMap[purchase.supplierId] = supplierResponse.data;
                        }
                    })
                );

                setSuppliers(suppliersMap);
            } catch {
                setError("Failed to load purchases.");
            }
        };

        fetchPurchases();
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
                                    Supplier
                                </TableCell>
                                <TableCell
                                    isHeader
                                    className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400"
                                >
                                    Purchase Date
                                </TableCell>
                                <TableCell
                                    isHeader
                                    className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400"
                                >
                                    Total
                                </TableCell>
                            </TableRow>
                        </TableHeader>

                        <TableBody className="divide-y divide-gray-100 dark:divide-white/[0.05]">
                            {purchases.map((purchase) => (
                                <TableRow key={purchase.id}>
                                    <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
                                        {suppliers[purchase.supplierId]?.name || "N/A"}
                                    </TableCell>
                                    <TableCell className="px-4 py-3 text-gray-500 text-theme-sm dark:text-gray-400">
                                        {new Date(purchase.purchaseDate).toLocaleString().slice(0, -3)}
                                    </TableCell>
                                    <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
                                        {purchase.total} &#8364;
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
