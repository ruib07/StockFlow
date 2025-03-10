import { useEffect, useState } from "react";
import { IAdministrator } from "../../@types/administrator";
import { useAdmin } from "../../hooks/useAdmin";
import { useDeleteModal } from "../../hooks/useDeleteModal";
import { useModal } from "../../hooks/useModal";
import { EyeCloseIcon, EyeIcon, PencilIcon, TrashBinIcon } from "../../icons";
import { DeleteAccount, UpdateAdmin } from "../../services/administratorsService";
import Input from "../form/input/InputField";
import Label from "../form/Label";
import Button from "../ui/button/Button";
import { Modal } from "../ui/modal";

export default function UserMetaCard() {
    const { isOpen, modalId, openModal, closeModal } = useModal();
    const { isDeleteOpen, deleteModalId, openDeleteModal, closeDeleteModal } = useDeleteModal();
    const [showPassword, setShowPassword] = useState(false);
    const [editedInfo, setEditedInfo] = useState<Partial<IAdministrator>>({
        name: "",
        email: "",
        password: "",
    });
    const { admin, error } = useAdmin();

    useEffect(() => {
        if (isOpen && modalId && admin) {
            const selectedAdmin = admin.id === modalId;
            if (selectedAdmin) {
                setEditedInfo({
                    name: admin.name,
                    email: admin.email,
                    password: admin.password
                });
            }
        }
    }, [isOpen, modalId, admin]);

    const handleSave = async () => {
        if (modalId) {
            if (!admin) return;
            await UpdateAdmin(modalId, editedInfo);
            closeModal();
        }
    };

    const handleDeleteAccount = async () => {
        if (deleteModalId) {
            if (!admin) return;
            await DeleteAccount(deleteModalId);
            closeDeleteModal();
        }
    }

    if (error) {
        return <p className="text-red-500">{error}</p>;
    }

    return (
        <>
            <div className="p-5 border border-gray-200 rounded-2xl dark:border-gray-800 lg:p-6">
                {admin ? (
                    <div className="flex flex-col gap-5 xl:flex-row xl:items-center xl:justify-between">
                        <div className="flex flex-col items-center w-full gap-6 xl:flex-row">
                            <div className="order-3 xl:order-2">
                                <h4 className="mb-2 text-lg font-semibold text-center text-gray-800 dark:text-white/90 xl:text-left">
                                    {admin.name}
                                </h4>
                                <div className="flex flex-col items-center gap-1 text-center xl:flex-row xl:gap-3 xl:text-left">
                                    <p className="text-sm text-gray-500 dark:text-gray-400">
                                        {admin.email}
                                    </p>
                                </div>
                            </div>
                        </div>
                        <button
                            onClick={() => openModal(admin.id)}
                            className="flex w-full items-center justify-center gap-2 rounded-full border border-gray-300 bg-white px-4 py-3 text-sm font-medium text-gray-700 shadow-theme-xs hover:bg-gray-50 hover:text-gray-800 dark:border-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:hover:bg-white/[0.03] dark:hover:text-gray-200 lg:inline-flex lg:w-auto"
                        >
                            <PencilIcon className="fill-gray-500 dark:fill-gray-400 size-5" />
                            Edit
                        </button>
                        <button
                            onClick={() => openDeleteModal(admin.id)}
                            className="flex w-full items-center justify-center gap-2 rounded-full border border-gray-300 bg-white px-4 py-3 text-sm font-medium text-gray-700 shadow-theme-xs hover:bg-gray-50 hover:text-gray-800 dark:border-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:hover:bg-white/[0.03] dark:hover:text-gray-200 lg:inline-flex lg:w-auto"
                        >
                            <TrashBinIcon className="fill-gray-500 dark:fill-gray-400 size-5" />
                            Account
                        </button>
                    </div>
                ) : (
                    <p>Loading data...</p>
                )}
            </div>
            <Modal isOpen={isOpen} onClose={closeModal} className="max-w-[700px] m-4">
                <div className="no-scrollbar relative w-full max-w-[700px] overflow-y-auto rounded-3xl bg-white p-4 dark:bg-gray-900 lg:p-11">
                    <div className="px-2 pr-14">
                        <h4 className="mb-2 text-2xl font-semibold text-gray-800 dark:text-white/90">
                            Edit Personal Information
                        </h4>
                        <p className="mb-6 text-sm text-gray-500 dark:text-gray-400 lg:mb-7">
                            Update your details to keep your profile up-to-date.
                        </p>
                    </div>
                    <form className="flex flex-col">
                        <div className="custom-scrollbar h-[450px] overflow-y-auto px-2 pb-3">
                            <div className="mt-7">
                                <h5 className="mb-5 text-lg font-medium text-gray-800 dark:text-white/90 lg:mb-6">
                                    Personal Information
                                </h5>

                                <div className="grid grid-cols-1 gap-x-6 gap-y-5 lg:grid-cols-2">
                                    <div className="col-span-2 lg:col-span-1">
                                        <Label>Name</Label>
                                        <Input
                                            type="text"
                                            value={editedInfo.name || admin?.name}
                                            onChange={(e) => setEditedInfo({ ...editedInfo, name: e.target.value })}
                                        />
                                    </div>

                                    <div className="col-span-2 lg:col-span-1">
                                        <Label>Email Address</Label>
                                        <Input
                                            type="email"
                                            value={editedInfo.email || admin?.email}
                                            onChange={(e) => setEditedInfo({ ...editedInfo, email: e.target.value })}
                                        />
                                    </div>

                                    <div className="col-span-2 lg:col-span-1">
                                        <Label>Password</Label>
                                        <div className="relative">
                                            <Input
                                                type={showPassword ? "text" : "password"}
                                                placeholder="Enter your password"
                                                value={editedInfo.password}
                                                onChange={(e) => setEditedInfo({ ...editedInfo, password: e.target.value })}
                                            />
                                            <span
                                                onClick={() => setShowPassword(!showPassword)}
                                                className="absolute z-30 -translate-y-1/2 cursor-pointer right-4 top-1/2"
                                            >
                                                {showPassword ? (
                                                    <EyeIcon className="fill-gray-500 dark:fill-gray-400 size-5" />
                                                ) : (
                                                    <EyeCloseIcon className="fill-gray-500 dark:fill-gray-400 size-5" />
                                                )}
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="flex items-center gap-3 px-2 mt-6 lg:justify-end">
                            <Button size="sm" variant="outline" onClick={closeModal}>
                                Close
                            </Button>
                            <Button size="sm" onClick={handleSave}>
                                Save Changes
                            </Button>
                        </div>
                    </form>
                </div>
            </Modal>
            <Modal isOpen={isDeleteOpen} onClose={closeDeleteModal} className="max-w-[700px] m-4">
                <div className="no-scrollbar relative w-full max-w-[700px] overflow-y-auto rounded-3xl bg-white p-4 dark:bg-gray-900 lg:p-11">
                    <div className="px-2 pr-14 text-center">
                        <h4 className="mb-2 text-2xl font-semibold text-gray-800 dark:text-white/90">
                            Delete Account
                        </h4>
                        <p className="mb-6 text-sm text-gray-500 dark:text-gray-400 lg:mb-7">
                            Are you sure you want to delete this account?
                        </p>
                    </div>
                    <form className="flex flex-col">
                        <div className="flex items-center gap-3 px-2 mt-6 lg:justify-center">
                            <Button size="sm" onClick={handleDeleteAccount}>
                                Yes, delete
                            </Button>
                            <Button size="sm" variant="outline" onClick={closeDeleteModal}>
                                No
                            </Button>
                        </div>
                    </form>
                </div>
            </Modal>
        </>
    );
}
