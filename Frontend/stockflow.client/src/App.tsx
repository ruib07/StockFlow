import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import AppLayout from "./layouts/AppLayout";
import Home from "./pages/Home";
import SignIn from "./pages/AuthPages/SignIn";
import SignUp from "./pages/AuthPages/SignUp";
import NotFound from "./pages/NotFound";
import UserProfiles from "./pages/UserProfiles";
import ProductsTable from "./pages/ProductsTable";

export default function App() {
    return (
        <Router>
            <ToastContainer />
            <Routes>
                <Route element={<AppLayout />}>
                    <Route index path="/" element={<Home />} />

                    <Route path="/profile" element={<UserProfiles />} />

                    <Route path="/products" element={<ProductsTable /> } />

                    <Route path="/signin" element={<SignIn />} />
                    <Route path="/signup" element={<SignUp />} />

                    <Route path="*" element={<NotFound />} />
                </Route>
            </Routes>
        </Router>
    )
}