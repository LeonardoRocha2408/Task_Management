import { Routes, Route } from "react-router-dom";
import ProtectedRoute from "./AuthContext/ProtectedRoute"
import CreateAccount from "./pages/AuthPages/CreateAccount";
import Login from "./pages/AuthPages/Login";
import Home from "./pages/HomePage/Home";
import Projects from "./pages/Projects/Projects";
import './App.css'

function App() {


    return (
        <>
            <Routes>

                <Route path="/login" element={<Login />} />
                <Route path="/create_account" element={<CreateAccount />} />

                <Route
                    path="/home"
                    element={
                        <ProtectedRoute>
                            <Home />
                        </ProtectedRoute>
                    }
                />
            </Routes>

        </>
    );
}


export default App
