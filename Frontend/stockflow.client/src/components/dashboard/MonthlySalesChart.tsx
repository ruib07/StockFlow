import { ApexOptions } from "apexcharts";
import { useEffect, useState } from "react";
import Chart from "react-apexcharts";
import { GetSales } from "../../services/salesService";

export default function MonthlySalesChart() {
    const [salesData, setSalesData] = useState<number[]>(Array(12).fill(0));
    const [, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchSales = async () => {
            try {
                const salesResponse = await GetSales();
                const sales = salesResponse.data;

                const monthlySales = Array(12).fill(0);
                sales.forEach((sale: { saleDate: string }) => {
                    const month = new Date(sale.saleDate).getMonth();
                    monthlySales[month] += 1;
                });

                setSalesData(monthlySales);
            } catch {
                setError("Failed to load sales.");
            }
        };

        fetchSales();
    }, []);

    const options: ApexOptions = {
        colors: ["#465fff"],
        chart: {
            fontFamily: "Outfit, sans-serif",
            type: "bar",
            height: 180,
            toolbar: {
                show: false,
            },
        },
        plotOptions: {
            bar: {
                horizontal: false,
                columnWidth: "39%",
                borderRadius: 5,
                borderRadiusApplication: "end",
            },
        },
        dataLabels: {
            enabled: false,
        },
        stroke: {
            show: true,
            width: 4,
            colors: ["transparent"],
        },
        xaxis: {
            categories: [
                "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
            ],
            axisBorder: { show: false },
            axisTicks: { show: false },
        },
        legend: {
            show: true,
            position: "top",
            horizontalAlign: "left",
            fontFamily: "Outfit",
        },
        yaxis: {
            title: { text: undefined },
        },
        grid: {
            yaxis: {
                lines: { show: true },
            },
        },
        fill: { opacity: 1 },
        tooltip: {
            x: { show: false },
            y: { formatter: (val: number) => `${val}` },
        },
    };
    const series = [
        {
            name: "Sales",
            data: salesData,
        },
    ];

    return (
        <div className="overflow-hidden rounded-2xl border border-gray-200 bg-white px-5 pt-5 dark:border-gray-800 dark:bg-white/[0.03] sm:px-6 sm:pt-6">
            <div className="flex items-center justify-between">
                <h3 className="text-lg font-semibold text-gray-800 dark:text-white/90">
                    Monthly Sales
                </h3>
            </div>

            <div className="max-w-full overflow-x-auto custom-scrollbar">
                <div className="-ml-5 min-w-[650px] xl:min-w-full pl-2">
                    <Chart options={options} series={series} type="bar" height={180} />
                </div>
            </div>
        </div>
    );
}
