import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

import AppLayout from "./layouts/AppLayout";
import Home from "./pages/Home";

import UserProfiles from "./pages/UserProfiles";

import CategoriesTable from "./pages/Tables/CategoriesTable";
import AddCategory from "./components/categories/AddCategory";

import ProductsTable from "./pages/Tables/ProductsTable";
import AddProduct from "./components/products/AddProduct";

import PurchasesTable from "./pages/Tables/PurchasesTable";
import AddPurchase from "./components/purchases/AddPurchase";

import SalesTable from "./pages/Tables/SalesTable";
import AddSale from "./components/sales/AddSale";

import CustomersTable from "./pages/Tables/CustomersTable";
import AddCustomer from "./components/customers/AddCustomer";

import SuppliersTable from "./pages/Tables/SuppliersTable";
import AddSupplier from "./components/suppliers/AddSupplier";

import SignIn from "./pages/AuthPages/SignIn";
import SignUp from "./pages/AuthPages/SignUp";
import RecoverPasswordEmail from "./components/recoverpassword/SendEmailForm";
import ChangePassword from "./components/recoverpassword/ChangePasswordForm";

import NotFound from "./pages/NotFound";


export default function App() {
    return (
        <Router>
            <ToastContainer />
            <Routes>
                <Route element={<AppLayout />}>
                    <Route index path="/" element={<Home />} />

                    <Route path="/profile" element={<UserProfiles />} />

                    <Route path="/categories" element={<CategoriesTable />} />
                    <Route path="/addcategory" element={<AddCategory /> } />

                    <Route path="/products" element={<ProductsTable />} />
                    <Route path="/addproduct" element={<AddProduct />} />

                    <Route path="/purchases" element={<PurchasesTable /> } />
                    <Route path="/addpurchase" element={<AddPurchase />} />

                    <Route path="/sales" element={<SalesTable /> } />
                    <Route path="/addsale" element={<AddSale /> } />

                    <Route path="/customers" element={<CustomersTable />} />
                    <Route path="/addcustomer" element={<AddCustomer />} />

                    <Route path="/suppliers" element={<SuppliersTable />} />
                    <Route path="/addsupplier" element={<AddSupplier /> } />

                    <Route path="/signin" element={<SignIn />} />
                    <Route path="/signup" element={<SignUp />} />
                    <Route path="/reset-password" element={<RecoverPasswordEmail />} />
                    <Route path="/change-password" element={<ChangePassword /> } />

                    <Route path="*" element={<NotFound />} />
                </Route>
            </Routes>
        </Router>
    )
}