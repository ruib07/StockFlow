import { useEffect, useState } from "react";
import { IAdministrator } from "../@types/administrator";
import { GetAdminById } from "../services/administratorsService";

export function useAdmin() {
    const [admin, setAdmin] = useState<IAdministrator | null>(null);
    const [error, setError] = useState<string | null>(null);

    const adminId = localStorage.getItem("adminId") || sessionStorage.getItem("adminId");

    useEffect(() => {
        if (!adminId) {
            setError("No admin ID found.");
            return;
        }

        const fetchAdmin = async () => {
            try {
                const adminResponse = await GetAdminById(adminId!);
                setAdmin(adminResponse.data);
                setError(null);
            } catch {
                setError("Failed to load admin data.");
                setAdmin(null);
            }
        };

        fetchAdmin();
    }, [adminId]);

    return { admin, error };
}