import { useEffect, useState } from "react";
import { ICategory } from "../../@types/category";
import { useDeleteModal } from "../../hooks/useDeleteModal";
import { useModal } from "../../hooks/useModal";
import { PencilIcon, TrashBinIcon } from "../../icons";
import { DeleteCategory, GetCategories, UpdateCategory } from "../../services/categoriesService";
import Label from "../form/Label";
import Input from "../form/input/InputField";
import Button from "../ui/button/Button";
import { Modal } from "../ui/modal";
import {
    Table,
    TableBody,
    TableCell,
    TableHeader,
    TableRow,
} from "../ui/table";

export default function CategoriesTableOne() {
    const { isOpen, modalId, openModal, closeModal } = useModal();
    const { isDeleteOpen, deleteModalId, openDeleteModal, closeDeleteModal } = useDeleteModal();
    const [categories, setCategories] = useState<ICategory[]>([]);
    const [, setError] = useState<string | null>(null);
    const [editedCategory, setEditedCategory] = useState<Partial<ICategory>>({ name: "" });

    useEffect(() => {
        const fetchCategories = async () => {
            try {
                const categoriesReponse = await GetCategories();
                setCategories(categoriesReponse.data);
            } catch {
                setError("Failed to load categories.");
            }
        };

        fetchCategories();
    }, []);

    useEffect(() => {
        if (isOpen && modalId) {
            const selectedCategory = categories.find(category => category.id === modalId);
            if (selectedCategory) {
                setEditedCategory({
                    name: selectedCategory.name
                });
            }
        }
    }, [isOpen, modalId, categories]);

    const handleEditCategory = async () => {
        if (modalId) {
            const updatedCategory = { ...editedCategory };
            await UpdateCategory(modalId, updatedCategory);
            closeModal();
        }
    };

    const handleDeleteCategory = async () => {
        if (deleteModalId) {
            await DeleteCategory(deleteModalId);
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
                                    Actions
                                </TableCell>
                            </TableRow>
                        </TableHeader>

                        <TableBody className="divide-y divide-gray-100 dark:divide-white/[0.05]">
                            {categories.map((category) => (
                                <TableRow key={category.id}>
                                    <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
                                        {category.name}
                                    </TableCell>
                                    <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
                                        <div className="flexitems-center gap-3">
                                            <div className="flex gap-2">
                                                <button
                                                    onClick={() => openModal(category.id)}
                                                    className="flex w-full items-center justify-center gap-2 rounded-full border border-gray-300 bg-white px-4 py-3 text-sm font-medium text-gray-700 shadow-theme-xs hover:bg-gray-50 hover:text-gray-800 dark:border-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:hover:bg-white/[0.03] dark:hover:text-gray-200 lg:inline-flex lg:w-auto"
                                                >
                                                    <PencilIcon className="fill-gray-500 dark:fill-gray-400 size-5" />
                                                </button>
                                                <button
                                                    onClick={() => openDeleteModal(category.id)}
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
                                                    Edit Category Information
                                                </h4>
                                                <p className="mb-6 text-sm text-gray-500 dark:text-gray-400 lg:mb-7">
                                                    Update your category details.
                                                </p>
                                            </div>
                                            <form className="flex flex-col">
                                                <div className="custom-scrollbar h-[450px] overflow-y-auto px-2 pb-3">
                                                    <div className="mt-7">
                                                        <h5 className="mb-5 text-lg font-medium text-gray-800 dark:text-white/90 lg:mb-6">
                                                            Category Information
                                                        </h5>

                                                        <div className="grid grid-cols-1 gap-x-6 gap-y-5 lg:grid-cols-2">
                                                            <div className="col-span-2 lg:col-span-2">
                                                                <Label>Name</Label>
                                                                <Input
                                                                    type="text"
                                                                    value={editedCategory.name || category?.name}
                                                                    onChange={(e) => setEditedCategory({ ...editedCategory, name: e.target.value })}
                                                                />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div className="flex items-center gap-3 px-2 mt-6 lg:justify-end">
                                                    <Button size="sm" variant="outline" onClick={closeModal}>
                                                        Close
                                                    </Button>
                                                    <Button size="sm" onClick={handleEditCategory}>
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
                                                    Delete Category
                                                </h4>
                                                <p className="mb-6 text-sm text-gray-500 dark:text-gray-400 lg:mb-7">
                                                    Are you sure you want to delete this category?
                                                </p>
                                            </div>
                                            <form className="flex flex-col">
                                                <div className="flex items-center gap-3 px-2 mt-6 lg:justify-center">
                                                    <Button size="sm" onClick={handleDeleteCategory}>
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
