import { Link } from "react-router-dom";
import { useState } from "react";
import validatePassword from "../.././utils/ValidatePassword";
import "./CreateAccount.css";

function CreateAccount() {
    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const validation = validatePassword(password);

   
    return (
        <>
            <div className="auth-page">
                
                <form
                    className="receive_data-form">
                    <img src="../.././images/form_Image.png"
                        className="form_img"/>
                    <h1>Create your account</h1>


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
                            type="password"
                            placeholder="Type your password"
                            value={password}
                            onChange={e => setPassword(e.target.value)}></input>
                    </div>

                    <p><b>Password requirements</b></p>
                    <div className="validation-password_div">
                        <p> {validation.hasMinLength ? "✔" : "✗"}
                            At least 8 characters</p>

                        <p> {validation.hasUpperCase ? "✔" : "✗"}
                            One uppercase letter (A-Z)</p>

                        <p> {validation.hasLowerCase ? "✔" : "✗"}
                            One lowercase letter (a-z)</p>

                        <p> {validation.hasNumber ? "✔" : "✗"}
                            One number (0-9)</p>

                        <p> {validation.hasSpecialCharacter ? "✔" : "✗"}
                            One special character</p>
                    </div>

                    <button type="submit">Create account</button>
                </form>
            </div>
        </>
  );
}
export default CreateAccount;
