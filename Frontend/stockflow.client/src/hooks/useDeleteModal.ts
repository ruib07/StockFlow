import { useState, useCallback } from "react";

export const useDeleteModal = (initialState: boolean = false) => {
    const [isDeleteOpen, setIsDeleteOpen] = useState(initialState);
    const [deleteModalId, setDeleteModalId] = useState<string | null>(null);

    const openDeleteModal = useCallback((id: string | null = null) => {
        setDeleteModalId(id);
        setIsDeleteOpen(true);
    }, []);

    const closeDeleteModal = useCallback(() => {
        setIsDeleteOpen(false);
        setDeleteModalId(null);
    }, []);

    const toggleDeleteModal = useCallback(() => {
        setIsDeleteOpen((prev) => !prev);
    }, []);

    return { isDeleteOpen, deleteModalId, openDeleteModal, closeDeleteModal, toggleDeleteModal };
};
