import { FormEvent, useState } from "react";
import { Link, useNavigate } from "react-router";
import { ISignup } from "../../@types/authentication";
import { ChevronLeftIcon, EyeCloseIcon, EyeIcon } from "../../icons";
import { Signup } from "../../services/authenticationsService";
import { showErrorToast, showSuccessToast } from "../../utils/toastHelper";
import Label from "../form/Label";
import Checkbox from "../form/input/Checkbox";
import Input from "../form/input/InputField";

export default function SignUpForm() {
    const [name, setName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [showPassword, setShowPassword] = useState(false);
    const [isChecked, setIsChecked] = useState(false);
    const navigate = useNavigate();

    const handleSignup = async (e: FormEvent) => {
        e.preventDefault();

        const newAdmin: ISignup = {
            name,
            email,
            password
        };

        try {
            await Signup(newAdmin);
            showSuccessToast();
            navigate("/signin");
        } catch {
            showErrorToast();
        }
    }

    return (
        <div className="flex flex-col flex-1 w-full overflow-y-auto lg:w-1/2 no-scrollbar">
            <div className="w-full max-w-md mx-auto mb-5 sm:pt-10">
                <Link
                    to="/"
                    className="inline-flex items-center text-sm text-gray-500 transition-colors hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-300"
                >
                    <ChevronLeftIcon className="size-5" />
                    Back to dashboard
                </Link>
            </div>
            <div className="flex flex-col justify-center flex-1 w-full max-w-md mx-auto">
                <div>
                    <div className="mb-5 sm:mb-8">
                        <h1 className="mb-2 font-semibold text-gray-800 text-title-sm dark:text-white/90 sm:text-title-md">
                            Sign Up
                        </h1>
                    </div>
                    <div>
                        <form onSubmit={handleSignup}>
                            <div className="space-y-5">
                                <div>
                                    <Label>
                                        Name<span className="text-error-500">*</span>
                                    </Label>
                                    <Input
                                        type="text"
                                        placeholder="Enter your name"
                                        value={name}
                                        onChange={(e) => setName(e.target.value)}
                                    />
                                </div>
                                <div>
                                    <Label>
                                        Email<span className="text-error-500">*</span>
                                    </Label>
                                    <Input
                                        type="email"
                                        placeholder="Enter your email"
                                        value={email}
                                        onChange={(e) => setEmail(e.target.value)}
                                    />
                                </div>
                                <div>
                                    <Label>
                                        Password<span className="text-error-500">*</span>
                                    </Label>
                                    <div className="relative">
                                        <Input
                                            placeholder="Enter your password"
                                            type={showPassword ? "text" : "password"}
                                            value={password}
                                            onChange={(e) => setPassword(e.target.value)}
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
                                <div className="flex items-center gap-3">
                                    <Checkbox
                                        className="w-5 h-5"
                                        checked={isChecked}
                                        onChange={setIsChecked}
                                    />
                                    <p className="inline-block font-normal text-gray-500 dark:text-gray-400">
                                        By creating an account means you agree to the{" "}
                                        <span className="text-gray-800 dark:text-white/90">
                                            Terms and Conditions,
                                        </span>{" "}
                                        and our{" "}
                                        <span className="text-gray-800 dark:text-white">
                                            Privacy Policy
                                        </span>
                                    </p>
                                </div>
                                <div>
                                    <button className="flex items-center justify-center w-full px-4 py-3 text-sm font-medium text-white transition rounded-lg bg-brand-500 shadow-theme-xs hover:bg-brand-600">
                                        Sign Up
                                    </button>
                                </div>
                            </div>
                        </form>

                        <div className="mt-5">
                            <p className="text-sm font-normal text-center text-gray-700 dark:text-gray-400 sm:text-start">
                                Already have an account? {""}
                                <Link
                                    to="/signin"
                                    className="text-brand-500 hover:text-brand-600 dark:text-brand-400"
                                >
                                    Sign In
                                </Link>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
