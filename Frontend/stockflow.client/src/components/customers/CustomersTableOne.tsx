import { useEffect, useState } from "react";
import { ICustomer } from "../../@types/customer";
import { useDeleteModal } from "../../hooks/useDeleteModal";
import { useModal } from "../../hooks/useModal";
import { PencilIcon, TrashBinIcon } from "../../icons";
import { DeleteCustomer, GetCustomers, UpdateCustomer } from "../../services/customersService";
import Label from "../form/Label";
import PhoneInput from "../form/group-input/PhoneInput";
import Input from "../form/input/InputField";
import TextArea from "../form/input/TextArea";
import Button from "../ui/button/Button";
import { Modal } from "../ui/modal";
import {
    Table,
    TableBody,
    TableCell,
    TableHeader,
    TableRow,
} from "../ui/table";

export default function CustomersTableOne() {
    const { isOpen, modalId, openModal, closeModal } = useModal();
    const { isDeleteOpen, deleteModalId, openDeleteModal, closeDeleteModal } = useDeleteModal();
    const [customers, setCustomers] = useState<ICustomer[]>([]);
    const [, setError] = useState<string | null>(null);
    const [editedCustomer, setEditedCustomer] = useState<Partial<ICustomer>>({
        name: "",
        nif: "",
        phoneNumber: "",
        email: "",
        address: ""
    });

    useEffect(() => {
        const fetchCustomers = async () => {
            try {
                const customersResponse = await GetCustomers();
                setCustomers(customersResponse.data);
            } catch {
                setError("Failed to load customers.");
            }
        };

        fetchCustomers();
    }, []);

    useEffect(() => {
        if (isOpen && modalId) {
            const selectedCustomer = customers.find(customer => customer.id === modalId);
            if (selectedCustomer) {
                setEditedCustomer({
                    name: selectedCustomer.name,
                    nif: selectedCustomer.nif,
                    phoneNumber: selectedCustomer.phoneNumber,
                    email: selectedCustomer.email,
                    address: selectedCustomer.address
                });
            }
        }
    }, [isOpen, modalId, customers]);

    const handleEditCustomer = async () => {
        if (modalId) {
            const updatedCustomer = { ...editedCustomer };
            await UpdateCustomer(modalId, updatedCustomer);
            closeModal();
        }
    };

    const handleDeleteCustomer = async () => {
        if (deleteModalId) {
            await DeleteCustomer(deleteModalId);
            closeDeleteModal();
        }
    }

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
                                    Name
                                </TableCell>
                                <TableCell
                                    isHeader
                                    className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400"
                                >
                                    NIF
                                </TableCell>
                                <TableCell
                                    isHeader
                                    className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400"
                                >
                                    PhoneNumber
                                </TableCell>
                                <TableCell
                                    isHeader
                                    className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400"
                                >
                                    Email
                                </TableCell>
                                <TableCell
                                    isHeader
                                    className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400"
                                >
                                    Address
                                </TableCell>
                                <TableCell
                                    isHeader
                                    className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400"
                                >
                                    Actions
                                </TableCell>
                            </TableRow>
                        </TableHeader>

                        <TableBody className="divide-y divide-gray-100 dark:divide-white/[0.05]">
                            {customers.map((customer) => (
                                <TableRow key={customer.id}>
                                    <TableCell className="px-4 py-3 text-gray-500 text-theme-sm dark:text-gray-400">
                                        {customer.name}
                                    </TableCell>
                                    <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
                                        {customer.nif}
                                    </TableCell>
                                    <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
                                        {customer.phoneNumber}
                                    </TableCell>
                                    <TableCell className="px-4 py-3 text-gray-500 text-theme-sm dark:text-gray-400">
                                        {customer.email}
                                    </TableCell>
                                    <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
                                        {customer.address}
                                    </TableCell>
                                    <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
                                        <div className="flexitems-center gap-3">
                                            <div className="flex gap-2">
                                                <button
                                                    onClick={() => openModal(customer.id)}
                                                    className="flex w-full items-center justify-center gap-2 rounded-full border border-gray-300 bg-white px-4 py-3 text-sm font-medium text-gray-700 shadow-theme-xs hover:bg-gray-50 hover:text-gray-800 dark:border-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:hover:bg-white/[0.03] dark:hover:text-gray-200 lg:inline-flex lg:w-auto"
                                                >
                                                    <PencilIcon className="fill-gray-500 dark:fill-gray-400 size-5" />
                                                </button>
                                                <button
                                                    onClick={() => openDeleteModal(customer.id)}
                                                    className="flex w-full items-center justify-center gap-2 rounded-full border border-gray-300 bg-white px-4 py-3 text-sm font-medium text-gray-700 shadow-theme-xs hover:bg-gray-50 hover:text-gray-800 dark:border-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:hover:bg-white/[0.03] dark:hover:text-gray-200 lg:inline-flex lg:w-auto"
                                                >
                                                    <TrashBinIcon className="fill-gray-500 dark:fill-gray-400 size-5" />
                                                </button>
                                            </div>
                                        </div>
                                    </TableCell>
                                    <Modal isOpen={isOpen} onClose={closeModal} className="max-w-[700px] m-4">
                                        <div className="no-scrollbar relative w-full max-w-[700px] overflow-y-auto rounded-3xl bg-white p-4 dark:bg-gray-900 lg:p-11">
                                            <div className="px-2 pr-14">
                                                <h4 className="mb-2 text-2xl font-semibold text-gray-800 dark:text-white/90">
                                                    Edit Customer Information
                                                </h4>
                                                <p className="mb-6 text-sm text-gray-500 dark:text-gray-400 lg:mb-7">
                                                    Update your customer details.
                                                </p>
                                            </div>
                                            <form className="flex flex-col">
                                                <div className="custom-scrollbar h-[450px] overflow-y-auto px-2 pb-3">
                                                    <div className="mt-7">
                                                        <h5 className="mb-5 text-lg font-medium text-gray-800 dark:text-white/90 lg:mb-6">
                                                            Customer Information
                                                        </h5>

                                                        <div className="grid grid-cols-1 gap-x-6 gap-y-5 lg:grid-cols-2">
                                                            <div className="col-span-2 lg:col-span-2">
                                                                <Label>Name</Label>
                                                                <Input
                                                                    type="text"
                                                                    value={editedCustomer.name || customer?.name}
                                                                    onChange={(e) => setEditedCustomer({ ...editedCustomer, name: e.target.value })}
                                                                />
                                                            </div>

                                                            <div className="col-span-2 lg:col-span-2">
                                                                <Label>NIF</Label>
                                                                <Input
                                                                    type="text"
                                                                    value={editedCustomer.nif || customer?.nif}
                                                                    onChange={(e) => setEditedCustomer({ ...editedCustomer, nif: e.target.value })}
                                                                />
                                                            </div>

                                                            <div className="col-span-2 lg:col-span-1">
                                                                <Label>Phone Number</Label>
                                                                <PhoneInput
                                                                    selectPosition="end"
                                                                    value={editedCustomer.phoneNumber || customer?.phoneNumber}
                                                                    onChange={(value) => setEditedCustomer({ ...editedCustomer, phoneNumber: value })}
                                                                />
                                                            </div>

                                                            <div className="col-span-2 lg:col-span-1">
                                                                <Label>Email</Label>
                                                                <Input
                                                                    type="email"
                                                                    value={editedCustomer.email || customer?.email}
                                                                    onChange={(e) => setEditedCustomer({ ...editedCustomer, email: e.target.value })}
                                                                />
                                                            </div>

                                                            <div className="col-span-2">
                                                                <Label>Address</Label>
                                                                <TextArea
                                                                    value={editedCustomer.address || customer?.address}
                                                                    onChange={(value) => setEditedCustomer({ ...editedCustomer, address: value })}
                                                                    rows={6}
                                                                />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div className="flex items-center gap-3 px-2 mt-6 lg:justify-end">
                                                    <Button size="sm" variant="outline" onClick={closeModal}>
                                                        Close
                                                    </Button>
                                                    <Button size="sm" onClick={handleEditCustomer}>
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
                                                    Delete Customer
                                                </h4>
                                                <p className="mb-6 text-sm text-gray-500 dark:text-gray-400 lg:mb-7">
                                                    Are you sure you want to delete this customer?
                                                </p>
                                            </div>
                                            <form className="flex flex-col">
                                                <div className="flex items-center gap-3 px-2 mt-6 lg:justify-center">
                                                    <Button size="sm" onClick={handleDeleteCustomer}>
                                                        Yes, delete
                                                    </Button>
                                                    <Button size="sm" variant="outline" onClick={closeDeleteModal}>
                                                        No
                                                    </Button>
                                                </div>
                                            </form>
                                        </div>
                                    </Modal>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </div>
            </div>
        </div>
    );
}
