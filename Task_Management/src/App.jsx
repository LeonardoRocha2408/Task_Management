import { Routes, Route } from "react-router-dom";

import CreateAccount from "./pages/CreateAccount/CreateAccount";
import './App.css'

function App() {


    return (
        <>
            <Routes>
                <Route path="/create_account" element={<CreateAccount />} />
            </Routes>

        </>
    );
}


export default App
