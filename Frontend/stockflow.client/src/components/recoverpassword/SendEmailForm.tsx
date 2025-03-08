import { useState } from "react";
import { Link } from "react-router-dom";
import { ISendEmail } from "../../@types/authentication";
import { ChevronLeftIcon } from "../../icons";
import { SendEmail } from "../../services/recoverPasswordsService";
import { showErrorToast, showSuccessToast } from "../../utils/toastHelper";
import Input from "../form/input/InputField";
import Label from "../form/Label";
import Button from "../ui/button/Button";

export default function RecoverPasswordEmail() {
    const [email, setEmail] = useState("");

    const handleEmailSending = async (e: React.FormEvent) => {
        e.preventDefault();

        const sendEmail: ISendEmail = { email };

        try {
            await SendEmail(sendEmail);
            showSuccessToast();
        } catch {
            showErrorToast();
        }
    }

    return (
        <div className="flex flex-col flex-1">
            <div className="pt-10 justify-start">
                <Link
                    to="/signin"
                    className="inline-flex text-sm text-gray-500 transition-colors hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-300"
                >
                    <ChevronLeftIcon className="size-5" />
                    Back to signin
                </Link>
            </div>
            <div className="flex flex-col justify-center flex-1 w-full max-w-md mx-auto">
                <div>
                    <div className="mb-5 sm:mb-8">
                        <h1 className="mb-2 font-semibold text-gray-800 text-title-sm dark:text-white/90 sm:text-title-md">
                            Recover Password
                        </h1>
                    </div>
                    <div>
                        <form onSubmit={handleEmailSending}>
                            <div className="space-y-6">
                                <div>
                                    <Label>
                                        Email <span className="text-error-500">*</span>{" "}
                                    </Label>
                                    <Input
                                        placeholder="info@gmail.com"
                                        value={email}
                                        onChange={(e) => setEmail(e.target.value)}
                                    />
                                </div>
                                <div>
                                    <Button className="w-full" size="sm">
                                        Send Email
                                    </Button>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    );
}
