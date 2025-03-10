import { useState, useCallback } from "react";

export const useModal = (initialState: boolean = false) => {
    const [isOpen, setIsOpen] = useState(initialState);
    const [modalId, setModalId] = useState<string | null>(null); 

    const openModal = useCallback((id: string | null = null) => {
        setModalId(id); 
        setIsOpen(true);
    }, []);

    const closeModal = useCallback(() => {
        setIsOpen(false);
        setModalId(null); 
    }, []);

    const toggleModal = useCallback(() => {
        setIsOpen((prev) => !prev);
    }, []);

    return { isOpen, modalId, openModal, closeModal, toggleModal };
};
