function validatePassword(password) {
    return {
        hasMinLength: password.length >= 8,
        hasUpperCase: /[A-Z]/.test(password),
        hasLowerCase: /[a-z]/.test(password),
        hasNumber: /[0-9]/.test(password),
        hasSpecialCharacter: /[!@#$%^&*(),.?":{}|<>]/.test(password)
    };
}

export default validatePassword;