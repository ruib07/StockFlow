import SignUpForm from "../../components/auth/SignUpForm";
import PageMeta from "../../components/common/PageMeta";
import AuthLayout from "./AuthPageLayout";

export default function SignUp() {
    return (
        <>
            <PageMeta
                title="Admin Signup Form"
                description="This is the admin signup form to create an admin"
            />
            <AuthLayout>
                <SignUpForm />
            </AuthLayout>
        </>
    );
}
