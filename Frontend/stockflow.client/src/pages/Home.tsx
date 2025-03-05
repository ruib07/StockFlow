import Metrics from "../components/dashboard/Metrics";
import MonthlySalesChart from "../components/dashboard/MonthlySalesChart";
import StatisticsChart from "../components/dashboard/StatisticsChart";
import MonthlyTarget from "../components/dashboard/MonthlyTarget";
import RecentOrders from "../components/dashboard/RecentOrders";
import PageMeta from "../components/common/PageMeta";

export default function Home() {
    const admin = localStorage.getItem("adminId") || sessionStorage.getItem("adminId");

    return (
        <>
            <PageMeta
                title="Admin Dashboard"
                description="This is a React Dashboard page"
            />
            <div>
                {!admin ? (
                    <div className="p-6 bg-red-200 rounded-xl text-gray-900 shadow-md text-center">
                        <div className="font-semibold text-xl">
                            Need to authenticate in order to see your stats
                        </div>
                    </div>
                ) : (
                    <div className="grid grid-cols-12 gap-4 md:gap-6">
                        <div className="col-span-12 space-y-6 xl:col-span-7">
                            <Metrics />

                            <MonthlySalesChart />
                        </div>

                        <div className="col-span-12 xl:col-span-5">
                            <MonthlyTarget />
                        </div>

                        <div className="col-span-12">
                            <StatisticsChart />
                        </div>

                        <div className="col-span-12">
                            <RecentOrders />
                        </div>
                    </div>
                )}
            </div>
        </>
    );
}
