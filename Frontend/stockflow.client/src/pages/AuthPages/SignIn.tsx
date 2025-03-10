import SignInForm from "../../components/auth/SignInForm";
import PageMeta from "../../components/common/PageMeta";
import AuthLayout from "./AuthPageLayout";

export default function SignIn() {
    return (
        <>
            <PageMeta
                title="Admin Signin Form"
                description="This is the admin signin form to authenticate an existing admin"
            />
            <AuthLayout>
                <SignInForm />
            </AuthLayout>
        </>
    );
}
