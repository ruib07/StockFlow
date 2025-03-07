import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { ISupplier } from "../../@types/supplier";
import { CreateSupplier } from "../../services/suppliersService";
import { showErrorToast, showSuccessToast } from "../../utils/toastHelper";
import ComponentCard from "../common/ComponentCard";
import PhoneInput from "../form/group-input/PhoneInput";
import Input from "../form/input/InputField";
import TextArea from "../form/input/TextArea";
import Label from "../form/Label";
import Button from "../ui/button/Button";

export default function AddSupplier() {
    const [name, setName] = useState("");
    const [nif, setNif] = useState("");
    const [phoneNumber, setPhoneNumber] = useState("");
    const [email, setEmail] = useState("");
    const [address, setAddress] = useState("");
    const navigate = useNavigate();

    const handleCustomerCreation = async (e: React.FormEvent) => {
        e.preventDefault();

        const newSupplier: ISupplier = {
            name,
            nif,
            phoneNumber,
            email,
            address
        };

        try {
            await CreateSupplier(newSupplier);
            showSuccessToast();

            navigate("/suppliers");
        } catch {
            showErrorToast();
        }
    }

    return (
        <ComponentCard title="Add Supplier">
            <form onSubmit={handleCustomerCreation}>
                <div className="space-y-6">
                    <div>
                        <Label htmlFor="input">Name</Label>
                        <Input type="text" value={name} onChange={(e) => setName(e.target.value)} />
                    </div>
                    <div>
                        <Label htmlFor="input">NIF</Label>
                        <Input
                            type="number"
                            value={nif}
                            onChange={(e) => {
                                const value = e.target.value;
                                if (value.length <= 9) {
                                    setNif(value);
                                }
                            }}
                        />
                    </div>

                    <div>
                        <Label htmlFor="input">Phone Number</Label>
                        <PhoneInput
                            selectPosition="end"
                            value={phoneNumber}
                            placeholder="+351 (555) 000-0000"
                            onChange={(value) => {
                                if (value.length <= 9) {
                                    setPhoneNumber(value);
                                }
                            }}
                        />
                    </div>
                    <div>
                        <Label htmlFor="inputTwo">Email</Label>
                        <Input type="text" value={email} placeholder="example@email.com" onChange={(e) => setEmail(e.target.value)} />
                    </div>
                    <div>
                        <Label>Address</Label>
                        <TextArea
                            value={address}
                            onChange={setAddress}
                            rows={2}
                        />
                    </div>
                    <div className="text-center">
                        <Button className="w-sm" size="sm">
                            Create Supplier
                        </Button>
                    </div>
                </div>
            </form>
        </ComponentCard>
    );
}
