import { createContext, useEffect, useState } from "react";

export const AuthContext = createContext(null);

export function AuthProvider({ children }) {
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        async function getMe() {
            try {
                const API_URL = import.meta.env.VITE_API_URL;

                const response = await fetch(`${API_URL}/me`, {
                    method: "GET",
                    credentials: "include"
                });

                if (!response.ok) {
                    setUser(null);
                    return;
                }

                const data = await response.json();
                setUser(data);

            } catch {
                setUser(null);
            } finally {
                setLoading(false);
            }
        }

        getMe();
    }, []);

    return (
        <AuthContext.Provider value={{ user, loading }}>
            {children}
        </AuthContext.Provider>
    );
}

export default AuthProvider;