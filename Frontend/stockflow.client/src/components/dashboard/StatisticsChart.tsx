import { ApexOptions } from "apexcharts";
import { useEffect, useState } from "react";
import Chart from "react-apexcharts";
import { GetSales } from "../../services/salesService";
import ChartTab from "../common/ChartTab";

export default function StatisticsChart() {
    const [salesData, setSalesData] = useState<number[]>(Array(12).fill(0));
    const [revenueData, setRevenueData] = useState<number[]>(Array(12).fill(0));
    const [, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchSales = async () => {
            try {
                const salesResponse = await GetSales();
                const sales = salesResponse.data;

                const monthlySales = Array(12).fill(0);
                const monthlyRevenue = Array(12).fill(0);

                sales.forEach((sale: { saleDate: string; total: number }) => {
                    const month = new Date(sale.saleDate).getMonth();
                    monthlySales[month] += 1;
                    monthlyRevenue[month] += sale.total;
                });

                setSalesData(monthlySales);
                setRevenueData(monthlyRevenue);
            } catch {
                setError("Failed to load sales.");
            }
        };

        fetchSales();
    }, []);

    const options: ApexOptions = {
        legend: {
            show: false,
            position: "top",
            horizontalAlign: "left",
        },
        colors: ["#465FFF", "#9CB9FF"],
        chart: {
            fontFamily: "Outfit, sans-serif",
            height: 310,
            type: "line", 
            toolbar: { show: false },
        },
        stroke: {
            curve: "straight", 
            width: [2, 2],
        },

        fill: {
            type: "gradient",
            gradient: {
                opacityFrom: 0.55,
                opacityTo: 0,
            },
        },
        markers: {
            size: 0, 
            strokeColors: "#fff", 
            strokeWidth: 2,
            hover: { size: 6 },
        },
        grid: {
            xaxis: {
                lines: { show: false },
            },
            yaxis: {
                lines: { show: true },
            },
        },
        dataLabels: { enabled: false },
        tooltip: {
            enabled: true, 
            x: { format: "dd MMM yyyy" },
        },
        xaxis: {
            type: "category", 
            categories: [
                "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
            ],
            axisBorder: { show: false },
            axisTicks: { show: false },
            tooltip: { enabled: false },
        },
        yaxis: {
            labels: {
                style: {
                    fontSize: "12px", 
                    colors: ["#6B7280"],
                },
            },
            title: {
                text: "", 
                style: { fontSize: "0px" },
            },
        },
    };

    const series = [
        {
            name: "Sales",
            data: salesData,
        },
        {
            name: "Revenue",
            data: revenueData,
        },
    ];
    return (
        <div className="rounded-2xl border border-gray-200 bg-white px-5 pb-5 pt-5 dark:border-gray-800 dark:bg-white/[0.03] sm:px-6 sm:pt-6">
            <div className="flex flex-col gap-5 mb-6 sm:flex-row sm:justify-between">
                <div className="w-full">
                    <h3 className="text-lg font-semibold text-gray-800 dark:text-white/90">
                        Statistics
                    </h3>
                    <p className="mt-1 text-gray-500 text-theme-sm dark:text-gray-400">
                        Target you’ve set for each month
                    </p>
                </div>
                <div className="flex items-start w-full gap-3 sm:justify-end">
                    <ChartTab />
                </div>
            </div>

            <div className="max-w-full overflow-x-auto custom-scrollbar">
                <div className="min-w-[1000px] xl:min-w-full">
                    <Chart options={options} series={series} type="area" height={310} />
                </div>
            </div>
        </div>
    );
}
