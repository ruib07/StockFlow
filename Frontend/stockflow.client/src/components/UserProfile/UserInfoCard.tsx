import { useAdmin } from "../../hooks/useAdmin";

export default function UserInfoCard() {
    const { admin, error } = useAdmin();

    if (error) {
        return <p className="text-red-500">{error}</p>;
    }

    return (
        <div className="p-5 border border-gray-200 rounded-2xl dark:border-gray-800 lg:p-6">
            <div className="flex flex-col gap-6 lg:flex-row lg:items-start lg:justify-between">
                {admin ? (
                    <div>
                        <h4 className="text-lg font-semibold text-gray-800 dark:text-white/90 lg:mb-6">
                            Personal Information
                        </h4>

                        <div className="grid grid-cols-1 gap-4 lg:grid-cols-2 lg:gap-7 2xl:gap-x-32">
                            <div>
                                <p className="mb-2 text-xs leading-normal text-gray-500 dark:text-gray-400">
                                    Name
                                </p>
                                <p className="text-sm font-medium text-gray-800 dark:text-white/90">
                                    {admin.name}
                                </p>
                            </div>

                            <div>
                                <p className="mb-2 text-xs leading-normal text-gray-500 dark:text-gray-400">
                                    Email address
                                </p>
                                <p className="text-sm font-medium text-gray-800 dark:text-white/90">
                                    {admin.email}
                                </p>
                            </div>
                        </div>
                    </div>
                ) : (
                    <p>Loading data...</p>
                )}
            </div>
        </div>
    );
}
