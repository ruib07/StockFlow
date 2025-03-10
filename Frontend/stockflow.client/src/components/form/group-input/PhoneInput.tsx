import { useEffect, useState } from "react";

interface PhoneInputProps {
    value: string;
    placeholder?: string;
    onChange?: (phoneNumber: string) => void;
    selectPosition?: "start" | "end";
}

const PhoneInput: React.FC<PhoneInputProps> = ({
    value,
    placeholder = "+351 (555) 000-0000",
    onChange,
    selectPosition = "start",
}) => {
    const [phoneNumber, setPhoneNumber] = useState<string>(value);

    useEffect(() => {
        setPhoneNumber(value);
    }, [value]);

    const handlePhoneNumberChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const newPhoneNumber = e.target.value.replace(/\D/g, "");
        if (newPhoneNumber.length <= 9) {
            setPhoneNumber(newPhoneNumber);
            if (onChange) {
                onChange(newPhoneNumber);
            }
        }
    };


    return (
        <div className="relative flex">
            {selectPosition === "start" && (
                <div className="absolute">
                    <div className="absolute inset-y-0 flex items-center text-gray-700 pointer-events-none bg-none right-3 dark:text-gray-400">
                        <svg
                            className="stroke-current"
                            width="20"
                            height="20"
                            viewBox="0 0 20 20"
                            fill="none"
                            xmlns="http://www.w3.org/2000/svg"
                        >
                            <path
                                d="M4.79175 7.396L10.0001 12.6043L15.2084 7.396"
                                stroke="currentColor"
                                strokeWidth="1.5"
                                strokeLinecap="round"
                                strokeLinejoin="round"
                            />
                        </svg>
                    </div>
                </div>
            )}

            <input
                type="tel"
                value={phoneNumber}
                onChange={handlePhoneNumberChange}
                placeholder={placeholder}
                className={`dark:bg-dark-900 h-11 w-full ${selectPosition === "start" ? "pl-[84px]" : "pr-[84px]"
                    } rounded-lg border border-gray-300 bg-transparent py-3 px-4 text-sm text-gray-800 shadow-theme-xs placeholder:text-gray-400 focus:border-brand-300 focus:outline-hidden focus:ring-3 focus:ring-brand-500/10 dark:border-gray-700 dark:bg-gray-900 dark:text-white/90 dark:placeholder:text-white/30 dark:focus:border-brand-800`}
            />

            {selectPosition === "end" && (
                <div className="absolute right-0">
                    <div className="absolute inset-y-0 flex items-center text-gray-700 pointer-events-none right-3 dark:text-gray-400">
                    </div>
                </div>
            )}
        </div>
    );
};

export default PhoneInput;
