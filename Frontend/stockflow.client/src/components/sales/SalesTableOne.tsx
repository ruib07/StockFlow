import { useEffect, useState } from "react";
import { ICustomer } from "../../@types/customer";
import { ISale } from "../../@types/sale";
import { GetCustomerById } from "../../services/customersService";
import { GetSales } from "../../services/salesService";
import {
    Table,
    TableBody,
    TableCell,
    TableHeader,
    TableRow,
} from "../ui/table";

export default function SalesTableOne() {
    const [sales, setSales] = useState<ISale[]>([]);
    const [customers, setCustomers] = useState<{ [key: string]: ICustomer }>({});
    const [, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchSales = async () => {
            try {
                const salesResponse = await GetSales();
                const salesData = salesResponse.data;
                setSales(salesData);

                const customersMap: { [key: string]: ICustomer } = {};

                await Promise.all(
                    salesData.map(async (sale: ISale) => {
                        if (sale.customerId && !customersMap[sale.customerId]) {
                            const customerResponse = await GetCustomerById(sale.customerId);
                            customersMap[sale.customerId] = customerResponse.data;
                        }
                    })
                );

                setCustomers(customersMap);
            } catch {
                setError("Failed to load sales.");
            }
        };

        fetchSales();
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
                                    Customer
                                </TableCell>
                                <TableCell
                                    isHeader
                                    className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400"
                                >
                                    Sale Date
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
                            {sales.map((sale) => (
                                <TableRow key={sale.id}>
                                    <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
                                        {customers[sale.customerId]?.name || "N/A"}
                                    </TableCell>
                                    <TableCell className="px-4 py-3 text-gray-500 text-theme-sm dark:text-gray-400">
                                        {new Date(sale.saleDate).toLocaleString().slice(0, -3)}
                                    </TableCell>
                                    <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
                                        {sale.total} &#8364;
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
