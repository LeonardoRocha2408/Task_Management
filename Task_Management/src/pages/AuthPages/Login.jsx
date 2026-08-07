import { Link } from "react-router-dom";
import { useState } from "react";
import validatePassword from "../../utils/ValidatePassword";
import "./AuthPage.css";

function Login() {
    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [showPassword, setShowPassword] = useState(false);
    const [dataError, setDataError] = useState("");

    async function sendData(e) {
        e.preventDefault();

        try {
            const response = await fetch("https://localhost:7161/login_account", {
                method: "POST",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    Email: email,
                    Password: password
                })
            });

            if (response.ok) {
                window.location.href = "/create_account"
            }
            else if (response.status >= 400 && response.status < 500) {
                setDataError("Data is incorrect");
                alert(response.status);
                console.log(response.status);
                return;
            }
        }
        catch (error) {
            alert(error)
        }
    }

    return (
        <>
            <div className="auth-page">

                <form
                    className="receive_data-form"
                    onSubmit={sendData}>


                    <img src="../.././images/form_Image.png"
                        className="form_img" />
                    <h1>Login your account</h1>
                    <p className="apresentation_p">Join Task Manager and start organizing <br></br>your tasks efficiently</p>

                    <p>Email</p>
                    <div className="organizes_input">
                        <label htmlFor="email"> <img src="../.././images/email.png" /> </label>
                        <input
                            id="email"
                            type="email"
                            placeholder="Type your email"
                            value={email}
                            onChange={e => setEmail(e.target.value)}></input>
                    </div>


                    <p>Password</p>
                    <div className="organizes_input">
                        <label htmlFor="password"><img src="../.././images/iconPassword.png" /></label>
                        <input
                            id="password"
                            type={showPassword ? "text" : "password"}
                            placeholder="Type your password"
                            value={password}
                            onChange={e => setPassword(e.target.value)}></input>

                        <button
                            type="button"
                            onClick={() => setShowPassword(!showPassword)}>
                            {showPassword ? "🙈" : "👁️"}</button>
                    </div>

                    <span className="error">{dataError}</span>


                    <button type="submit" className="send-data_button">Create account</button>

                    <span>or</span>
                    <nav>
                        <span>Don't have an account yet? <Link className="link" to="/create_account">Sign up</Link> </span>
                    </nav>
                </form>

            </div>
        </>
    );
}
export default Login;
