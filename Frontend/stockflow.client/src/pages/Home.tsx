import PageMeta from "../components/common/PageMeta";
import Metrics from "../components/dashboard/Metrics";
import MonthlyPurchasesChart from "../components/dashboard/MonthlyPurchasesChart";
import MonthlySalesChart from "../components/dashboard/MonthlySalesChart";
import RecentProducts from "../components/dashboard/RecentProducts";
import StatisticsChart from "../components/dashboard/StatisticsChart";

export default function Home() {
    const admin = localStorage.getItem("adminId") || sessionStorage.getItem("adminId");

    return (
        <>
            <PageMeta
                title="Admin Home Dashboard"
                description="This is a Dashboard page to show the statistics"
            />
            <div>
                {!admin ? (
                    <div className="p-6 bg-red-200 rounded-xl text-gray-900 shadow-md text-center">
                        <div className="font-semibold text-xl">
                            Need to authenticate in order to see the stats
                        </div>
                    </div>
                ) : (
                    <div className="grid grid-cols-12 gap-4 md:gap-6">
                        <div className="col-span-12 space-y-6 xl:col-span-8">
                            <MonthlyPurchasesChart />

                            <MonthlySalesChart />
                        </div>

                        <div className="col-span-12 xl:col-span-4">
                            <Metrics />
                        </div>

                        <div className="col-span-12">
                            <StatisticsChart />
                        </div>

                        <div className="col-span-12">
                            <RecentProducts />
                        </div>
                    </div>
                )}
            </div>
        </>
    );
}
