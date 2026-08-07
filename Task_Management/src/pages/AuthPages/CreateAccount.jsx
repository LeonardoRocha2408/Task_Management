import { Link } from "react-router-dom";
import { useState } from "react";
import validatePassword from "../../utils/ValidatePassword";
import "./AuthPage.css";

function CreateAccount() {
    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [showPassword, setShowPassword] = useState(false);
    const [dataError, setDataError] = useState("");
    const validation = validatePassword(password);

    async function sendData(e) {
        e.preventDefault();

        try {
            if (!email.trim() && !username.trim() && !password.trim()) {
                setDataError("Data is required");
                return;
            }
            if (!email.trim()) {
                setDataError("Email is required");
                return;
            }
            if (!username.trim()) {
                setDataError("Username is required");
                return;
            }
            if (!password.trim()) {
                setDataError("Password is required");
                return;
            }

            const response = await fetch("https://localhost:7161/create_account", {
                method: "POST",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    Email: email,
                    Username: username,
                    Password: password
                })
            });

            if (response.ok) {
                window.location.href = "/login"
            }
            else if (response.status >= 400 && response.status < 500) {
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
                        className="form_img"/>
                    <h1>Create your account</h1>
                    <p className="apresentation_p">Join Task Manager and start organizing <br></br>your tasks efficiently</p>

                    <p>Email</p>
                    <div className="organizes_input">
                        <label htmlFor="email"> <img src="../.././images/email.png"/> </label>
                        <input
                            id="email"
                            type="email"
                            placeholder="Type your email"
                            value={email}
                            onChange={e => setEmail(e.target.value)}></input>
                    </div>

                    <p>Username</p>
                    <div className="organizes_input">
                        <label htmlFor="username"><img src="../.././images/iconUser.png"/></label>
                        <input
                            id="username"
                            type="text"
                            placeholder="Type your username"
                            value={username}
                            onChange={e => setUsername(e.target.value)}></input>
                    </div>

                    <p>Password</p>
                    <div className="organizes_input">
                        <label htmlFor="password"><img src="../.././images/iconPassword.png"/></label>
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


                    <p><b>Password requirements</b></p>
                    <div className="validation-password_div">
                        <p> {validation.hasMinLength ? "✔ " : "✗ "}
                            At least 8 characters</p>

                        <p> {validation.hasUpperCase ? "✔ " : "✗ "}
                            One uppercase letter (A-Z)</p>

                        <p> {validation.hasLowerCase ? "✔ " : "✗ "}
                            One lowercase letter (a-z)</p>

                        <p> {validation.hasNumber ? "✔ " : "✗ "}
                            One number (0-9)</p>

                        <p> {validation.hasSpecialCharacter ? "✔ " : "✗ "}
                            One special character</p>
                    </div>


                    <button type="submit" className="send-data_button">Create account</button>

                    <span>or</span>
                    <nav>
                    <span>Already have an account? <Link className="link" to="/login">Sign in</Link> </span>
                    </nav>
                </form>
                
            </div>
        </>
  );
}
export default CreateAccount;
